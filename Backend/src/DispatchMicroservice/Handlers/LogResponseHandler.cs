using Azure.Messaging.ServiceBus;
using DispatchMicroservice.Hubs;
using Microsoft.AspNetCore.SignalR;
using Shared.DTO.Azure;
using Shared.Interfaces;

namespace DispatchMicroservice.Handlers
{
    public class LogResponseHandler
    {
        private readonly ILogger<LogResponse> _logger;
        private readonly IDeserializer<LogResponse> _deserializer;
        private readonly IHubContext<NotificationHub> _hubContext;
        public LogResponseHandler(ILogger<LogResponse> logger, IDeserializer<LogResponse> deserializer, IHubContext<NotificationHub> hubContext)
        {
            _logger = logger;
            _deserializer = deserializer;
            _hubContext = hubContext;
        }
        /// <summary>
        /// Handles responsed log to SignalR group and pushes it to the client. Method name: 'RecieveNotification'
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task HandleLogResponse(ProcessMessageEventArgs args)
        {
            var body = args.Message.Body.ToString();
            var responseLog = _deserializer.Deserialize(body);

            if (responseLog is null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(responseLog.OwnerId))
            {
                _logger.LogWarning(
                    "SignalR update skipped because OwnerId is missing. CorrelationId: {CorrelationId}",
                    responseLog.CorrelationId
                );

                return;
            }

            var groupName = NotificationHubGroups.User(responseLog.OwnerId);

            _logger.LogInformation(
                "Sending SignalR update. CorrelationId: {CorrelationId}, Status: {Status}",
                responseLog.CorrelationId,
                responseLog.Status
            );

            await _hubContext.Clients
                .Group(groupName)
                .SendAsync("ReceiveNotification", responseLog);
        }
        public Task HandleError(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Service Bus error occurred: {ErrorSource}", args.ErrorSource);
            return Task.CompletedTask;
        }
    }
}
