namespace finrv.Shared;


public class RequestInfo
{
    public string? CorrelationId { get; private set; }
    public string? ClientType { get; private set; }
    
    public string? UserId { get; private set; }

    public void SetInfo(string? correlationId, string? clientType, string? userId)
    {
        CorrelationId = correlationId;
        ClientType = clientType;
        UserId = userId;
    }
}
