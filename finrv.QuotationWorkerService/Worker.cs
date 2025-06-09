using finrv.QuotationWorkerService.Abstraction;
using finrv.QuotationWorkerService.Events;

namespace finrv.QuotationWorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IListener<QuotationUpdateEvent> _quotationUpdateListener;

    public Worker(ILogger<Worker> logger, IListener<QuotationUpdateEvent> quotationUpdateListener)
    {
        _logger = logger;
        _quotationUpdateListener = quotationUpdateListener;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Listening | {Class} | {Method} | Worker Service.", nameof(Worker), nameof(ExecuteAsync));
        _ = _quotationUpdateListener.StartAsync(stoppingToken);
    }
}