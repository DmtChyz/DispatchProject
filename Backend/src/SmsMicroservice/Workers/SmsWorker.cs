using Azure.Messaging.ServiceBus;

namespace SmsMicroservice.Workers
{
    public class SmsWorker : BackgroundService
    {
        private readonly ServiceBusClient _client;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmsWorker> _logger;

        private readonly string _topicName;
        private readonly string _subscriptionName;

        public SmsWorker(
            ServiceBusClient client,
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ILogger<SmsWorker> logger)
        {
            _configuration = configuration;
            _client = client;
            _scopeFactory = scopeFactory;
            _logger = logger;

            _topicName = _configuration["ServiceBus:TopicName"]!;
            _subscriptionName = _configuration["ServiceBus:SubscriptionName"]!;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var processor = _client.CreateProcessor(
                _topicName,
                _subscriptionName,
                new ServiceBusProcessorOptions
                {
                    AutoCompleteMessages = false
                });

            processor.ProcessMessageAsync += async args =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<SmsHandler>();
                    await handler.HandleSmsSending(args);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to process SMS message. EntityPath: {EntityPath}, MessageId: {MessageId}, CorrelationId: {CorrelationId}",
                        args.EntityPath,
                        args.Message.MessageId,
                        args.Message.CorrelationId);

                    await args.AbandonMessageAsync(args.Message);
                }
            };

            processor.ProcessErrorAsync += async args =>
            {
                using var scope = _scopeFactory.CreateScope();

                var handler = scope.ServiceProvider.GetRequiredService<SmsHandler>();

                await handler.HandleError(args);
            };

            await processor.StartProcessingAsync(stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}