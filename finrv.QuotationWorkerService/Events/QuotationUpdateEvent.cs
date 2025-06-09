namespace finrv.QuotationWorkerService.Events;

public record QuotationUpdateEvent(
    Guid Id,
    string CorrelationId,
    string Ticker,
    string Name,
    decimal Price,
    DateTime LatestUpdate);