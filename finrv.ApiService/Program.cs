using finrv.ApiService.Extensions;
using finrv.ApiService.Routes;
using finrv.Infra.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddProblemDetails();
builder.Services.AddDependencies();
builder.Services.AddServices();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapOpenApiDocumentation(builder.Configuration);
}

app.MapMiddlewares();

app.MapGroup("/api/v1/users")
    .MapUsers();

app.MapGroup("/api/v1/assets")
    .MapAssets();

app.MapGroup("/api/v1/transactions")
    .MapTransactions();

app.Run();
