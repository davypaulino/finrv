var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.finrv_QuotationWorkerService>("quotation-worker-service")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.finrv_ApiService>("fin-web-api-service");

builder.Build().Run();