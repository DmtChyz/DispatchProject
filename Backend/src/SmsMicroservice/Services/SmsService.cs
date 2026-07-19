using DispatchProject.Enumerable.ActionLog;
using Shared.DTO.Azure;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Messaging;

namespace SmsMicroservice.Services
{
    public class SmsService
    {
        private readonly ServiceBusPublisher _publisher;
        private readonly IConfiguration _configuration;
        public SmsService(ServiceBusPublisher publisher,IConfiguration configuration)
        {
            _publisher = publisher;
            _configuration = configuration;
        }
        public async Task SendSms(NotificationMessage message)
        {
            if (FieldsIsNull(message))
            {
                await _publisher.PublishLogAsync(message, ActionStatus.Failed);
                return;
            }
            await SendSms(message.Recipient, message.Details, message);
        }
        private static bool FieldsIsNull(NotificationMessage message)
        {
            if (message.ActionType == null
                || string.IsNullOrEmpty(message.Recipient)
                || string.IsNullOrEmpty(message.Details)
                || string.IsNullOrEmpty(message.CorrelationId))
            {
                return true;
            }
            return false;
        }
        private async Task SendSms(string recipientNumber,string details,NotificationMessage message)
        {
            recipientNumber = recipientNumber.Trim();
            var messageOptions = new CreateMessageOptions(new Twilio.Types.PhoneNumber(recipientNumber));
            messageOptions.From = new Twilio.Types.PhoneNumber(_configuration["Twilio:FromNumber"]);
            messageOptions.Body = "DispatchProject notification:" + details;
            try
            {
                await MessageResource.CreateAsync(messageOptions);
                await _publisher.PublishLogAsync(message, ActionStatus.Completed);
            }
            catch
            {
                await _publisher.PublishLogAsync(message, ActionStatus.Failed);
            }
        }
    }
}
