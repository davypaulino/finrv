using finrv.Shared;
using System.Runtime.CompilerServices;

namespace finrv.ApiService.Middlewares;

public class RequestInfoMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestInfoMiddleware> _logger;
    private const string CLASS_NAME = nameof(RequestInfoMiddleware);

    public RequestInfoMiddleware(RequestDelegate next, ILogger<RequestInfoMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context, RequestInfo requestInfo)
    {
        var correlationId = context.Request.Headers["X-Correlation-Id"].FirstOrDefault();
        var clientType = context.Request.Headers["X-Client-Type"].FirstOrDefault();

        _logger.LogInformation("Starting | Class {ClassName} | Method {Method} | Correlation: {CorrelationId} | ClientType: {ClientType} ",
            CLASS_NAME, nameof(Invoke), correlationId, clientType);

        requestInfo.SetInfo(
            context.Request.Headers["X-Correlation-Id"].FirstOrDefault(),
            context.Request.Headers["X-Client-Type"].FirstOrDefault());

        await _next(context);

        _logger.LogInformation("Finished | Class {ClassName} | Method {Method} | Correlation: {CorrelationId} | ClientType: {ClientType} ",
            CLASS_NAME, nameof(Invoke), correlationId, clientType);
    }
}
