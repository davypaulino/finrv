namespace finrv.QuotationWorkerService.Events;

public record QuotationUpdateEvent(
    Guid Id,
    string CorrelationId,
    string Ticker,
    string Price,
    DateTime LatestUpdate);