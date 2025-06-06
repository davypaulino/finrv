using finrv.Infra.Extensions.EnvironmentConfigMaps;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Scalar.AspNetCore;

namespace finrv.Infra.Extensions;

public static class OpenApiServiceConfig
{
    public static WebApplication MapOpenApiDocumentation(this WebApplication app, IConfiguration configuration)
    {
        app.MapScalarApiReference("/api-reference", options =>
        {
            var config = configuration.GetSection("OpeApiDocumentationSettings").Get<OpenApiDocumentationSettings>();
            options.WithTitle(config!.Title);
            options.WithFavicon(config!.Favicon);
        });

        return app;
    }
}
