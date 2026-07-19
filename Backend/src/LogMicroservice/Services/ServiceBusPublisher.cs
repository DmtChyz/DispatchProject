using Azure.Messaging.ServiceBus;
using DispatchProject.Enumerable.ActionLog;
using Shared.DTO.Azure;
using System.Text.Json;

namespace LogMicroservice.Services;

public class ServiceBusPublisher
{
    private readonly ServiceBusSender _sender;

    public ServiceBusPublisher(ServiceBusClient client)
    {
        _sender = client.CreateSender("signalr-log");
    }

    /// <summary>
    /// Publishes the processed notification result to the SignalR log queue.
    /// </summary>
    public async Task SendResponseLogAsync(
        string correlationId,
        string ownerId,
        DateTime date,
        ActionStatus status,
        ActionType actionType)
    {
        var responseLog = MapNotificationToResponseLog(
            correlationId,
            ownerId,
            date,
            status,
            actionType
        );

        var busMessage = new ServiceBusMessage(
            JsonSerializer.Serialize(responseLog)
        )
        {
            CorrelationId = correlationId
        };

        await _sender.SendMessageAsync(busMessage);
    }

    private static LogResponse MapNotificationToResponseLog(
        string correlationId,
        string ownerId,
        DateTime date,
        ActionStatus status,
        ActionType actionType)
    {
        return new LogResponse
        {
            CorrelationId = correlationId,
            OwnerId = ownerId,
            CreatedAt = date,
            Status = status,
            ActionType = actionType
        };
    }
}