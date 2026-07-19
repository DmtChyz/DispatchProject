using DispatchMicroservice.Hubs.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace DispatchMicroservice.Hubs
{
    [Authorize]
    public class NotificationHub : Hub<INotificationClient>
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning(
                    "SignalR connection rejected. ConnectionId: {ConnectionId}. UserId claim is missing.",
                    Context.ConnectionId
                );

                Context.Abort();
                return;
            }

            var groupName = NotificationHubGroups.User(userId);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrWhiteSpace(userId))
            {
                await Groups.RemoveFromGroupAsync(
                    Context.ConnectionId,
                    NotificationHubGroups.User(userId)
                );
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}