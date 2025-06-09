using Confluent.Kafka;
using finrv.Infra.Helpers;
using finrv.QuotationWorkerService.Abstraction;
using finrv.QuotationWorkerService.Settings;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace finrv.QuotationWorkerService.Consumers
{
    public class KafkaConsumerService<TEvent> where TEvent : class
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly ILogger<KafkaConsumerService<TEvent>> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly CustomDeserializer<TEvent> _deserializer;
        private readonly RetryPolicySettings _retryPolicySettings;
        private AsyncRetryPolicy _retryPolicy;
    
        public KafkaConsumerService(
            ConsumerConfig consumerConfig,
            ILogger<KafkaConsumerService<TEvent>> logger,
            IServiceScopeFactory scopeFactory,
            CustomDeserializer<TEvent> deserializer,
            IOptions<RetryPolicySettings> retryPolicySettings)
        {
            _logger = logger;
            _consumerConfig = consumerConfig;
            _scopeFactory = scopeFactory;
            _deserializer = deserializer;
            _retryPolicySettings = retryPolicySettings.Value;
        }

        public async Task SetupConsumerAsync(string topicName, CancellationToken cancellationToken)
        {
            using var consumer = new ConsumerBuilder<Ignore, TEvent>(_consumerConfig)
                .SetKeyDeserializer(Deserializers.Ignore)
                .SetValueDeserializer(_deserializer)
                .SetErrorHandler((_, e) => _logger.LogError($"Error on Kafka Consumer: {e.Reason}"))
                .SetLogHandler((_, l) => _logger.LogInformation($"Kafka Consumer: {l.Message}"))
                .Build();

            _logger.LogInformation($"Consumer Listener Topic: {topicName}");
            consumer.Subscribe(topicName);
            
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    _retryPolicySettings.RetryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(_retryPolicySettings.SleepDurationPower, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(exception, $"Falha ao processar mensagem Kafka. Tentando novamente em {timeSpan.TotalSeconds:N1} segundos. Tentativa {retryCount}/{_retryPolicySettings.RetryCount}.");
                    });
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        if (consumeResult == null)
                            continue;

                        if (consumeResult.IsPartitionEOF)
                        {
                            _logger.LogInformation(
                                "Partition Finished {Partition} | offset {Offset} | Topic {Topic}",
                                consumeResult.Partition, consumeResult.Offset, consumeResult.Topic);
                            continue;
                        }
                    
                        await ExtractConsumerMessage(consumer, consumeResult, cancellationToken);
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Erro ao consumir mensagem: {e.Error.Reason}");
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("Kafka consumer operation cancelled.");
                        break;
                    }
                    catch (Exception e)
                    {
                        _logger.LogCritical(e, "Critical unhandled exception in Kafka consumer loop.");
                    }
                }
            }
            finally
            {
                consumer.Close();
                _logger.LogInformation("Kafka consumer closed.");
            }
        }
    
        public async Task ExtractConsumerMessage(
            IConsumer<Ignore, TEvent>? consumer, 
            ConsumeResult<Ignore, TEvent>? consumeResult,
            CancellationToken cancellation)
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var listener = scope.ServiceProvider.GetRequiredService<IListener<TEvent>>();
                    try
                    {
                        TEvent? message = consumeResult.Message.Value;
                        await listener.ProcessMessageAsync(message);
                        consumer?.Commit(consumeResult);
                        _logger.LogInformation(
                            "Offset {Offset} commit on partition {Partition} with success.",
                            consumeResult.Offset, consumeResult.Partition);
                    }
                    catch (Exception ex)
                    {
                        listener.ProcessErrorAsync(consumeResult.Message.Value, ex);
                        throw;
                    }
                }
            });
        }
    }
}