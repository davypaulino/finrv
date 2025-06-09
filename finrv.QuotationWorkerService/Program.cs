using Confluent.Kafka;
using finrv.Application.Extensions;
using finrv.Infra.Extensions;
using finrv.Infra.Helpers;
using finrv.QuotationWorkerService;
using finrv.QuotationWorkerService.Abstraction;
using finrv.QuotationWorkerService.Consumers;
using finrv.QuotationWorkerService.Events;
using finrv.QuotationWorkerService.Listeners;
using finrv.QuotationWorkerService.Settings;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddLogging();
builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddDependencies();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Broker"));
builder.Services.Configure<RetryPolicySettings>(builder.Configuration.GetSection("RetryPolicy"));
builder.Services.AddSingleton<CustomDeserializer<QuotationUpdateEvent>>();
builder.Services.AddSingleton<ConsumerConfig>(serviceProvider =>
{
    var kafkaSettings = serviceProvider.GetRequiredService<IOptions<KafkaSettings>>().Value;
    return new ConsumerConfig
    {
        BootstrapServers = kafkaSettings.Host,
        GroupId = kafkaSettings.GroupId,
        AutoOffsetReset = AutoOffsetReset.Earliest,
        EnableAutoCommit = false,
        SessionTimeoutMs = 6000,
        EnablePartitionEof = true
    };
});

builder.Services.AddScoped<IListener<QuotationUpdateEvent>, QuotationUpdateListener>();
builder.Services.AddSingleton<KafkaConsumerService<QuotationUpdateEvent>>();

var host = builder.Build();
host.Run();