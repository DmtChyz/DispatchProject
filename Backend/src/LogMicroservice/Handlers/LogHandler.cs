using Azure.Messaging.ServiceBus;
using LogMicroservice.Services;
using Newtonsoft.Json;
using Shared.DTO.Azure;
using Shared.Interfaces;

namespace LogMicroservice.Handlers
{
    public class LogHandler
    {
        private readonly ILogger<LogHandler> _logger;
        private readonly LogService _logService;
        public LogHandler(ILogger<LogHandler> logger, LogService logService, IDeserializer<LogMessage> deserializer)
        {
            _logger = logger;
            _logService = logService;
        }
        // add queueName path from message to db Later.
        public async Task HandleLogSending(ProcessMessageEventArgs args)
        {
            var body = args.Message.Body.ToString();
            LogMessage? logMessage;

            try
            {
                logMessage = JsonConvert.DeserializeObject<LogMessage>(body);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "Received invalid JSON log message, skipping. Body: {Body}",
                    body);

                await args.CompleteMessageAsync(args.Message);
                return;
            }
            if (logMessage == null)
            {
                _logger.LogWarning("Received empty or null log message, skipping. Body: {Body}", body);
                await args.CompleteMessageAsync(args.Message);
                return;
            }

            await _logService.CreateLog(logMessage);

            await args.CompleteMessageAsync(args.Message);
        }
        public Task HandleError(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Service Bus error occurred: {ErrorSource}", args.ErrorSource);
            return Task.CompletedTask;
        }
    }
}
