using finrv.ApiService.Middlewares;
using finrv.Shared;
using System.Net;
using System.Text.Json;

namespace finrv.ApiService.Extensions;
public static class MiddlewareServicesConfig
{
    public static WebApplication MapMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<RequestInfoMiddleware>();
        app.MapGlobalExceptions();

        return app;
    }

    public static WebApplication MapGlobalExceptions(this WebApplication app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("GlobalExceptionHandler");

                var requestInfo = context.RequestServices.GetRequiredService<RequestInfo>();

                logger.LogError("Erro inesperado na requisição | ClientType: {ClientType} | CorrelationId: {CorrelationId} | Path: {Path}",
                    requestInfo.ClientType, requestInfo.CorrelationId, context.Request.Path.ToString().Sanitize());

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new { Message = "Erro inesperado" };
                var jsonResponse = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(jsonResponse);
            });
        });

        return app;
    }
}
