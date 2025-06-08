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
                var exceptionHandlerPathFeature =
                    context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;

                var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("GlobalExceptionHandler");
                
                var requestInfo = context.RequestServices.GetService<RequestInfo>();
                
                logger.LogError(exception,
                    "Erro inesperado na requisição | ClientType: {ClientType} | CorrelationId: {CorrelationId} | Path: {Path} | ExceptionType: {ExceptionType} | ExceptionMessage: {ExceptionMessage}",
                    requestInfo?.ClientType,
                    requestInfo?.CorrelationId,
                    context.Request.Path.ToString().Sanitize(),
                    exception?.GetType().Name,
                    exception?.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                string message = "Ocorreu um erro inesperado no servidor.";

                if (exception is BadHttpRequestException || exception is FormatException)
                {
                    message = "Um dos parâmetros fornecidos é inválido.";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }

                var response = new { Message = message };
                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            });
        });
        
        return app;
    }
}
