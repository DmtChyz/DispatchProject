using Azure.Messaging.ServiceBus;
using DispatchProject.Enumerable.ActionLog;
using Shared.DTO.Azure;
using System.Text.Json;

namespace SmsMicroservice.Services
{
    public class ServiceBusPublisher
    {
        private readonly ServiceBusSender _sender;
        private IConfiguration _configuration;
        private readonly ILogger<ServiceBusPublisher> _logger;

        public ServiceBusPublisher(ServiceBusClient client, IConfiguration configuration, ILogger<ServiceBusPublisher> logger)
        {
            _configuration = configuration;
            _sender = client.CreateSender(_configuration["ServiceBus:LogQueue"]);
            _logger = logger;

        }

        public async Task PublishLogAsync(
        NotificationMessage request,
        ActionStatus status)
        {
            var logMessage = MapNotificationToLog(request, status);
            var serializedLog = JsonSerializer.Serialize(logMessage);

            var busMessage = new ServiceBusMessage(serializedLog)
            {
                CorrelationId = request.CorrelationId
            };

            await _sender.SendMessageAsync(busMessage);
        }

        private static LogMessage MapNotificationToLog(NotificationMessage request, ActionStatus status)
        {
            return new LogMessage()
            {
                ActionType = request.ActionType,
                CorrelationId = request.CorrelationId,
                CreatedAt = DateTime.UtcNow,
                Details = request.Details,
                OwnerId = request.OwnerId,
                Recipient = request.Recipient,
                Status = status,
            };
        }
    }
}
