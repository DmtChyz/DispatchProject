using DispatchProject.Enumerable.ActionLog;
using MailKit.Net.Smtp;
using MimeKit;
using Shared.DTO.Azure;

namespace EmailMicroservice.Services
{
    public class EmailService
    {
        private readonly ServiceBusPublisher _publisher;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            ServiceBusPublisher publisher,
            IConfiguration configuration,
            ILogger<EmailService> logger)
        {
            _publisher = publisher;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmail(NotificationMessage message)
        {
            if (FieldsIsNull(message))
            {
                await _publisher.PublishLogAsync(message, ActionStatus.Failed);
                return;
            }

            await SendEmail(CreateEmail(message.Recipient, message.Details), message);
        }

        private MimeMessage CreateEmail(string recipient, string details)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Dispatch", _configuration["Email:FromEmail"]));
            message.To.Add(new MailboxAddress("", recipient));
            message.Subject = "Dispatch Notification";
            message.Body = new TextPart("html") { Text = details };

            return message;
        }

        /// <summary>
        /// Caller in charge of disposing object. ('using var')
        /// </summary>
        /// <returns></returns>
        private async Task<SmtpClient> GetSmtpClient()
        {
            var client = new SmtpClient();

            await client.ConnectAsync(
                _configuration["Email:SmtpHost"],
                int.Parse(_configuration["Email:SmtpPort"]!));

            await client.AuthenticateAsync(
                _configuration["Email:FromEmail"],
                _configuration["Email:SmtpPassword"]);

            return client;
        }

        private async Task SendEmail(MimeMessage message, NotificationMessage notificationData)
        {
            try
            {
                using var client = await GetSmtpClient();

                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to send email. Recipient: {Recipient}, CorrelationId: {CorrelationId}",
                    notificationData.Recipient,
                    notificationData.CorrelationId);

                await _publisher.PublishLogAsync(notificationData, ActionStatus.Failed);
                return;
            }

            await _publisher.PublishLogAsync(notificationData, ActionStatus.Completed);
        }

        private static bool FieldsIsNull(NotificationMessage message)
        {
            return string.IsNullOrWhiteSpace(message.Recipient)
                || string.IsNullOrWhiteSpace(message.Details)
                || string.IsNullOrWhiteSpace(message.CorrelationId);
        }
    }
}