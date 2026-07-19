using Azure.Messaging.ServiceBus;
using DispatchProject.Enumerable.ActionLog;
using Shared.DTO.Azure;
using Shared.Helper;
using Shared.Interfaces;
using SmsMicroservice.Services;

public class SmsHandler
{
    private readonly SmsService _smsService;
    private readonly IDeserializer<NotificationMessage> _deserializer;
    private readonly ILogger<SmsHandler> _logger;
    private readonly ServiceBusPublisher _publisher;

    public SmsHandler(
        SmsService smsService,
        IDeserializer<NotificationMessage> deserializer,
        ILogger<SmsHandler> logger,
        ServiceBusPublisher publisher)
    {
        _smsService = smsService;
        _deserializer = deserializer;
        _logger = logger;
        _publisher = publisher;
    }

    public async Task HandleSmsSending(ProcessMessageEventArgs args)
    {

        var body = args.Message.Body.ToString();
        var notificationMessage = _deserializer.Deserialize(body);


        if (notificationMessage == null)
        {
            _logger.LogWarning(
                "Received invalid SMS notification message. MessageId: {MessageId}, CorrelationId: {CorrelationId}, Body: {Body}",
                args.Message.MessageId,
                args.Message.CorrelationId,
                body);

            var fallbackMessage = Helper.FallbackDeserializer<NotificationMessage>(body);

            await _publisher.PublishLogAsync(fallbackMessage, ActionStatus.Failed);

            await args.CompleteMessageAsync(args.Message);
            return;
        }

        await _smsService.SendSms(notificationMessage);

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