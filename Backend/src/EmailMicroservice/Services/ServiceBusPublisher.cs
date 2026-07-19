using Azure.Messaging.ServiceBus;
using DispatchProject.Enumerable.ActionLog;
using Shared.DTO.Azure;
using System.Text.Json;

namespace EmailMicroservice.Services;

public class ServiceBusPublisher
{
    private readonly ServiceBusSender _sender;
    private readonly IConfiguration _configuration;

    public ServiceBusPublisher(ServiceBusClient client, IConfiguration configuration)
    {
        _configuration = configuration;
        _sender = client.CreateSender(_configuration["ServiceBus:LogQueue"]!);
    }

    public async Task PublishLogAsync(
        NotificationMessage request,
        ActionStatus status)
    {
        var logMessage = MapNotificationToLog(request, status);

        var busMessage = new ServiceBusMessage(
            JsonSerializer.Serialize(logMessage)
        )
        {
            CorrelationId = request.CorrelationId
        };

        await _sender.SendMessageAsync(busMessage);
    }

    private static LogMessage MapNotificationToLog(
        NotificationMessage request,
        ActionStatus status)
    {
        return new LogMessage
        {
            ActionType = request.ActionType,
            CorrelationId = request.CorrelationId,
            CreatedAt = DateTime.UtcNow,
            Details = request.Details,
            OwnerId = request.OwnerId,
            Recipient = request.Recipient,
            Status = status
        };
    }
}