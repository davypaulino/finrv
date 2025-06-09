using finrv.Shared;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace finrv.Application.Middlewares;

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
        var correlationId = context.Request.Headers["X-Correlation-Id"].FirstOrDefault()?.Sanitize();
        var clientType = context.Request.Headers["X-Client-Type"].FirstOrDefault()?.Sanitize();
        var userId = context.Request.Headers["X-User-Id"].FirstOrDefault()?.Sanitize();

        _logger.LogInformation("Starting | Class {ClassName} | Method {Method} | Correlation: {CorrelationId} | ClientType: {ClientType} ",
            CLASS_NAME, nameof(Invoke), correlationId, clientType);

        requestInfo.SetInfo(
            correlationId,
            clientType,
            userId);

        await _next(context);

        _logger.LogInformation("Finished | Class {ClassName} | Method {Method} | Correlation: {CorrelationId} | ClientType: {ClientType} ",
            CLASS_NAME, nameof(Invoke), correlationId, clientType);
    }
}
