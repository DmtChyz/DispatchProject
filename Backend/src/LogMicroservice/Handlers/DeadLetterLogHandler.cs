using Azure.Messaging.ServiceBus;
using LogMicroservice.Entities;
using LogMicroservice.Services;
using Shared.DTO.Azure;
using System.Text;
using System.Text.Json;

namespace LogMicroservice.Handlers
{
    public class DeadLetterLogHandler
    {
        private readonly ILogger<DeadLetterLogHandler> _logger;
        private readonly LogService _logService;
        public DeadLetterLogHandler(ILogger<DeadLetterLogHandler> logger, LogService logService)
        {
            _logger = logger;
            _logService = logService;
        }
        public async Task HandleDeadLetterLogSending(ProcessMessageEventArgs args, string queueName)
        {
            await _logService.CreateDeadLetterLog(ExtractMetaDataFromMessageEventArgs(args, queueName));
        }
        public Task HandleError(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Service Bus error occurred: {ErrorSource}", args.ErrorSource);
            return Task.CompletedTask;
        }
        public DlqLogMessage ExtractMetaDataFromMessageEventArgs(ProcessMessageEventArgs args, string queueName)
        {
            var log = new DlqLogMessage();

            log.QueueName = queueName; // from worker
            log.Reason = string.IsNullOrEmpty(args.Message.DeadLetterReason)
                ? null
                : args.Message.DeadLetterReason;

            log.ErrorDescription = string.IsNullOrEmpty(args.Message.DeadLetterErrorDescription)
                ? null
                : args.Message.DeadLetterErrorDescription;

            log.EnqueuedAt = args.Message.EnqueuedTime == default // structure
                ? DateTime.UtcNow
                : args.Message.EnqueuedTime.UtcDateTime;

            var rawJson = Encoding.UTF8.GetString(args.Message.Body.ToArray());

            // if json body don't have correlationId we assign default message correlationId
            log.CorrelationId = args.Message.CorrelationId;

            // extract user correlatioId from json - top priority
            try
            {
                // converting to readable json
                using JsonDocument doc = JsonDocument.Parse(rawJson);

                log.MessageBody =
                    doc.RootElement.ValueKind == JsonValueKind.Object  // if JSON root is an object like { }
                    &&                                                 // and
                    !doc.RootElement.EnumerateObject().Any()           // doesn't have any property
                    ? null
                    : JsonSerializer.Serialize(doc.RootElement);

                
                if (doc.RootElement.ValueKind == JsonValueKind.Object)
                {
                    if (doc.RootElement.TryGetProperty("CorrelationId", out var pascalCorrelationIdElement))
                    {
                        var correlationId = pascalCorrelationIdElement.GetString();

                        if (!string.IsNullOrWhiteSpace(correlationId))
                            log.CorrelationId = correlationId;
                    }
                    else if (doc.RootElement.TryGetProperty("correlationId", out var camelCorrelationIdElement))
                    {
                        var correlationId = camelCorrelationIdElement.GetString();

                        if (!string.IsNullOrWhiteSpace(correlationId))
                            log.CorrelationId = correlationId;
                    }
                }
            }
            catch
            {
                log.MessageBody = rawJson;

                // 5. No need to set log.CorrelationId here because metadata fallback was already set before JSON parsing.
            }

            return log;
        }
    }
}
