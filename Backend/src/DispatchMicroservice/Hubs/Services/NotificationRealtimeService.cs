using DispatchMicroservice.Hubs.Contracts;
using Microsoft.AspNetCore.SignalR;
using Shared.Constants;
using Shared.DTO.Contracts.SignalRResponses;

namespace DispatchMicroservice.Hubs.Services;

public class NotificationRealtimeService
{
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

    public NotificationRealtimeService(IHubContext<NotificationHub, INotificationClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendNotificationStatusChangedAsync(
        string userId,
        string notificationId,
        string status,
        string? correlationId = null)
    {
        var code = status switch
        {
            "queued" => ResponseCodes.Notification.Queued,
            "processing" => ResponseCodes.Notification.Processing,
            "sent" => ResponseCodes.Notification.Sent,
            "failed" => ResponseCodes.Notification.Failed,
            _ => ResponseCodes.Notification.Failed
        };

        var message = RealtimeMessage<NotificationStatusChangedDto>.Ok(code, new NotificationStatusChangedDto(notificationId,status), correlationId);

        await _hubContext.Clients
            .Group(NotificationHubGroups.User(userId))
            .NotificationStatusChanged(message);
    }
}