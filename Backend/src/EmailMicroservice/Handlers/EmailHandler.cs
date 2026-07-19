using Azure.Messaging.ServiceBus;
using DispatchProject.Enumerable.ActionLog;
using EmailMicroservice.Services;
using Shared.DTO.Azure;
using Shared.Helper;
using Shared.Interfaces;

namespace EmailMicroservice.Handlers
{
    public class EmailHandler
    {
        private readonly EmailService _emailService;
        private readonly ServiceBusPublisher _publisher;
        private readonly IDeserializer<NotificationMessage> _deserializer;
        private readonly ILogger<EmailHandler> _logger;

        public EmailHandler(
            ServiceBusPublisher publisher,
            EmailService emailService,
            IDeserializer<NotificationMessage> deserializer,
            ILogger<EmailHandler> logger)
        {
            _publisher = publisher;
            _emailService = emailService;
            _deserializer = deserializer;
            _logger = logger;
        }

        public async Task HandleEmailSending(ProcessMessageEventArgs args)
        {
            var body = args.Message.Body.ToString();

            var notificationMessage = _deserializer.Deserialize(body);

            if (notificationMessage == null)
            {
                _logger.LogWarning(
                    "Received invalid email notification message. MessageId: {MessageId}, CorrelationId: {CorrelationId}, Body: {Body}",
                    args.Message.MessageId,
                    args.Message.CorrelationId,
                    body);

                await _publisher.PublishLogAsync(
                    Helper.FallbackDeserializer<NotificationMessage>(body),
                    ActionStatus.Failed);

                await args.CompleteMessageAsync(args.Message);
                return;
            }

            await _emailService.SendEmail(notificationMessage);

            await args.CompleteMessageAsync(args.Message);
        }

        public Task HandleError(ProcessErrorEventArgs args)
        {
            _logger.LogError(
                args.Exception,
                "Service Bus error occurred: {ErrorSource}",
                args.ErrorSource);

            return Task.CompletedTask;
        }
    }
}