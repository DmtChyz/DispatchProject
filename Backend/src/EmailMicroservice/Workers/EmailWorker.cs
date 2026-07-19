using Azure.Messaging.ServiceBus;
using EmailMicroservice.Handlers;

namespace EmailMicroservice.Workers
{
    public class EmailWorker : BackgroundService
    {
        private readonly ServiceBusClient _client;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailWorker> _logger;

        private readonly string _topicName;
        private readonly string _subscriptionName;

        public EmailWorker(
            ServiceBusClient client,
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ILogger<EmailWorker> logger)
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

                    var handler = scope.ServiceProvider.GetRequiredService<EmailHandler>();
                    await handler.HandleEmailSending(args);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to process email message. EntityPath: {EntityPath}, MessageId: {MessageId}, CorrelationId: {CorrelationId}",
                        args.EntityPath,
                        args.Message.MessageId,
                        args.Message.CorrelationId);

                    await args.AbandonMessageAsync(args.Message);
                }
            };

            processor.ProcessErrorAsync += async args =>
            {
                using var scope = _scopeFactory.CreateScope();

                var handler = scope.ServiceProvider.GetRequiredService<EmailHandler>();

                await handler.HandleError(args);
            };

            await processor.StartProcessingAsync(stoppingToken);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}