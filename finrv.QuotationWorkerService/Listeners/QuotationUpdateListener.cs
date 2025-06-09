using System.Text.Json;
using Confluent.Kafka;
using finrv.QuotationWorkerService.Abstraction;
using finrv.QuotationWorkerService.Consumers;
using finrv.QuotationWorkerService.Events;
using finrv.QuotationWorkerService.Settings;
using Microsoft.Extensions.Options;

namespace finrv.QuotationWorkerService.Listeners;

public class QuotationUpdateListener : IListener<QuotationUpdateEvent>
{
    private readonly string? _kafkaTopic;
    private const string CLASS_NAME = nameof(QuotationUpdateListener);
    
    private readonly ILogger<QuotationUpdateListener> _logger;
    private KafkaConsumerService<QuotationUpdateEvent> _consumerService;
    
    public QuotationUpdateListener(
        ILogger<QuotationUpdateListener> logger,
        IOptions<KafkaSettings>  kafkaSettings,
        KafkaConsumerService<QuotationUpdateEvent> consumerService)
    {
        _logger = logger;
        _consumerService = consumerService;
        if (!kafkaSettings.Value.Topics.TryGetValue(CLASS_NAME, out _kafkaTopic))
        {
            throw new InvalidOperationException($"Kafka topic '{CLASS_NAME}' not found in settings.");
        }
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
         _logger.LogInformation("Starting | Class: {ClassName} | Method: {Method} | Listening event {Event} on topic {Topic}",
            CLASS_NAME, nameof(StartAsync), nameof(QuotationUpdateEvent), _kafkaTopic);
         await _consumerService.SetupConsumerAsync(this, _kafkaTopic, cancellationToken);
    }
    
    public async Task ProcessMessageAsync(QuotationUpdateEvent message)
    {
        _logger.LogInformation("Processing event | Class: {ClassName} | Method: {Method} | Event ID: {EventId}",
            CLASS_NAME, nameof(ProcessMessageAsync), message?.Id);

        _logger.LogInformation("Simulating processing of QuotationUpdateEvent for Symbol: {Symbol}, Price: {Price}",
            message?.Ticker, message?.Price);
        
        await Task.Delay(100);

        _logger.LogInformation("Event processed successfully | Event ID: {EventId}", message?.Id);
    }

    public Task ProcessErrorAsync(string rawMessage, Exception exception)
    {
        _logger.LogError(exception, "Error handling message. Raw Message: {RawMessage}", rawMessage);
        return Task.CompletedTask;
    }

    public Task ProcessInvalidMessageAsync(string rawMessage, Exception exception)
    {
        _logger.LogWarning(exception, "Received invalid message format. Raw Message: {RawMessage}", rawMessage);
        return Task.CompletedTask;
    }
}