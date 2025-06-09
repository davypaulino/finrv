namespace finrv.QuotationWorkerService.Abstraction;

public interface IListener<TEvent> where TEvent : class
{
    Task ProcessMessageAsync(TEvent message);
    void ProcessErrorAsync(TEvent rawMessage, Exception exception);
}