using System.Text.Json;
using Confluent.Kafka;
using finrv.QuotationWorkerService.Abstraction;

namespace finrv.QuotationWorkerService.Consumers;

public class KafkaConsumerService<TEvent> where TEvent : class
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly ILogger<KafkaConsumerService<TEvent>> _logger;
    private IListener<TEvent>?  _listener;
    
    public KafkaConsumerService(
        ConsumerConfig consumerConfig,
        ILogger<KafkaConsumerService<TEvent>> logger)
    {
        _logger = logger;
        _consumerConfig = consumerConfig ?? throw new ArgumentNullException(nameof(consumerConfig));
    }

    public async Task SetupConsumerAsync(IListener<TEvent> listener, string topicName, CancellationToken cancellationToken)
    {
        _listener = listener;
        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig)
            .SetErrorHandler((_, e) => _logger.LogError($"Error on Kafka Consumer: {e.Reason}"))
            .SetLogHandler((_, l) => _logger.LogInformation($"Kafka Consumer: {l.Message}"))
            .Build();

        _logger.LogInformation($"Consumer Listener Topic: {topicName}");
        consumer.Subscribe(topicName);

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
        IConsumer<Ignore, string>? consumer, 
        ConsumeResult<Ignore, string>? consumeResult,
        CancellationToken cancellation)
    {
        string rawMessage = consumeResult.Message.Value;
        
        _logger.LogDebug(
            "Received message | Topic: {Topic} | Partition: {Partition} | Offset: {Offset} | Key: {Key} | Value: {Value}",
            consumeResult.Topic, consumeResult.Partition, consumeResult.Offset,
            consumeResult.Message.Key, rawMessage);
        
        TEvent? messageEvent = null;
        try
        {
            messageEvent = JsonSerializer.Deserialize<TEvent>(rawMessage);
            if (messageEvent == null)
            {
                throw new JsonException(
                    $"Failed to deserialize message to {nameof(TEvent)}: result is null.");
            }
            
            await _listener.ProcessMessageAsync(messageEvent);
            
            consumer?.Commit(consumeResult);
            _logger.LogInformation(
                "Offset {Offset} commitado para a partição {Partition} após processamento bem-sucedido.",
                consumeResult.Offset, consumeResult.Partition);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize Kafka message. Raw message: {RawMessage}",
                rawMessage);
            await _listener.ProcessInvalidMessageAsync(rawMessage, ex);
            consumer?.Commit(consumeResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing Kafka message. Raw message: {RawMessage}",
                rawMessage);
            await _listener.ProcessErrorAsync(rawMessage, ex);
        }
    }
}