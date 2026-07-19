using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Shared.Deserialization;
using Shared.Interfaces;
using SmsMicroservice.Services;
using SmsMicroservice.Workers;
using Twilio;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<ServiceBusClient>(new ServiceBusClient(builder.Configuration.GetConnectionString("AzureServicePrimaryKey")));

TwilioClient.Init(
    builder.Configuration["Twilio:AccountSid"],
    builder.Configuration["Twilio:AuthToken"]
);

builder.Services.AddScoped<SmsService>();
builder.Services.AddScoped<SmsHandler>();
builder.Services.AddHostedService<SmsWorker>();  // singleton but needs scoped each time, so we need to use factory \\  
builder.Services.AddSingleton<ServiceBusPublisher>();

builder.Services.AddScoped(
    typeof(IDeserializer<>),
    typeof(JsonMessageDeserializer<>));

var host = builder.Build();
host.Run();
