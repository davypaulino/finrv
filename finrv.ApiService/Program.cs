using finrv.ApiService.Routes;
using finrv.Infra.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapOpenApiDocumentation(builder.Configuration);
}

app.MapGroup("/api/v1/users")
    .MapUsers();

app.Run();
