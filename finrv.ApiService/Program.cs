using finrv.ApiService.Extensions.EnvironmentConfigMaps;
using finrv.ApiService.Routes;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api-reference", options =>
    {
        var config = builder.Configuration.GetSection("OpeApiDocumentationSettings").Get<OpenApiDocumentationSettings>();
        options.WithTitle(config!.Title);
        options.WithFavicon(config!.Favicon);
    });
}

app.MapGroup("/api/v1/users")
    .MapUsers();

app.Run();
