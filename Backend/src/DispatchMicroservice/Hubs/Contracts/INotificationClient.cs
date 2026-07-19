using Shared.DTO.Contracts.SignalRResponses;

namespace DispatchMicroservice.Hubs.Contracts
{
    public interface INotificationClient
    {
        Task NotificationStatusChanged(
            RealtimeMessage<NotificationStatusChangedDto> message
        );
    }
}
