using finrv.QuotationWorkerService.Consumers;
using finrv.QuotationWorkerService.Events;
using finrv.QuotationWorkerService.Listeners;
using finrv.QuotationWorkerService.Settings;
using Microsoft.Extensions.Options;

namespace finrv.QuotationWorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly KafkaSettings _kafkaSettings;
    private readonly KafkaConsumerService<QuotationUpdateEvent> _kafkaConsumerService;

    public Worker(
        IOptions<KafkaSettings> kafkaSettings,
        ILogger<Worker> logger,
        KafkaConsumerService<QuotationUpdateEvent> consumerService)
    {
        _logger = logger;
        _kafkaSettings = kafkaSettings.Value;
        _kafkaConsumerService = consumerService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Listening | {Class} | {Method} | Worker Service.", nameof(Worker), nameof(ExecuteAsync));
        _ = _kafkaConsumerService.SetupConsumerAsync(_kafkaSettings.Topics[nameof(QuotationUpdateListener)], stoppingToken);
    }
}