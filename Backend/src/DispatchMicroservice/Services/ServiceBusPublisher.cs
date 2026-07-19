using Azure.Messaging.ServiceBus;
using DispatchProject.Enumerable.ActionLog;
using Shared.DTO.Azure;
using System.Text.Json;

namespace DispatchMicroservice.Services
{
    public sealed class ServiceBusPublisher : IAsyncDisposable
    {
        private readonly ServiceBusSender _sender;

        public ServiceBusPublisher(ServiceBusClient client)
        {
            _sender = client.CreateSender("dispatch-notifications");
        }

        public async Task PublishAsync(NotificationMessage request)
        {
            var busMessage = new ServiceBusMessage(JsonSerializer.Serialize(request));

            busMessage.ApplicationProperties["ActionType"] = GetPropertyName(request.ActionType);
            busMessage.CorrelationId = request.CorrelationId;

            await _sender.SendMessageAsync(busMessage);
        }

        private static string GetPropertyName(ActionType messageType)
        {
            return messageType.ToString();
        }

        public async ValueTask DisposeAsync()
        {
            await _sender.DisposeAsync();
        }
    }
}