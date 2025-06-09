namespace finrv.QuotationWorkerService.Abstraction;

public interface IListener<TEvent> where TEvent : class
{
    Task StartAsync(CancellationToken cancellationToken);
    Task ProcessMessageAsync(TEvent message);
    Task ProcessErrorAsync(string rawMessage, Exception exception);
    Task ProcessInvalidMessageAsync(string rawMessage, Exception exception);
}