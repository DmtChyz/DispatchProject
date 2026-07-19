using Azure.Messaging.ServiceBus;
using LogMicroservice.Data;
using LogMicroservice.Handlers;
using LogMicroservice.Services;
using LogMicroservice.Workers;
using Microsoft.EntityFrameworkCore;
using Shared.Deserialization;
using Shared.DTO;
using Shared.Interfaces;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<ServiceBusClient>(new ServiceBusClient(builder.Configuration.GetConnectionString("AzureServicePrimaryKey")));


builder.Services.AddSingleton<ServiceBusPublisher>();
builder.Services.AddScoped<LogService>();

builder.Services.AddScoped<LogHandler>();
builder.Services.AddHostedService<LogWorker>();

builder.Services.AddHostedService<DeadLetterLogWorker>();
builder.Services.AddScoped<DeadLetterLogHandler>();

builder.Services.AddScoped(
    typeof(IDeserializer<>),
    typeof(JsonMessageDeserializer<>));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
var host = builder.Build();
host.Run();
