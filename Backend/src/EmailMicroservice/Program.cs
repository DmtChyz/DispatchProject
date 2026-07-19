using Azure.Messaging.ServiceBus;
using EmailMicroservice.Handlers;
using EmailMicroservice.Services;
using EmailMicroservice.Workers;
using Microsoft.EntityFrameworkCore;
using Shared.Deserialization;
using Shared.Interfaces;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<ServiceBusClient>(new ServiceBusClient(builder.Configuration.GetConnectionString("AzureServicePrimaryKey")));

builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<EmailHandler>();
builder.Services.AddHostedService<EmailWorker>();  // singleton but needs scoped each time, so we need to use factory \\  
builder.Services.AddSingleton<ServiceBusPublisher>();


builder.Services.AddScoped(
    typeof(IDeserializer<>),
    typeof(JsonMessageDeserializer<>));

var host = builder.Build();
host.Run();
    