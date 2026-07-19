using Azure.Messaging.ServiceBus;
using DispatchMicroservice.Handlers;

namespace DispatchMicroservice.Workers
{
    public class LogResponseWorker : BackgroundService
    {
        private readonly ServiceBusClient _client;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LogResponseWorker> _logger;

        private readonly string _queueName;
        private ServiceBusProcessor? _processor;

        public LogResponseWorker(
            ServiceBusClient client,
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ILogger<LogResponseWorker> logger)
        {
            _client = client;
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _logger = logger;

            _queueName = _configuration["ServiceBus:Queue"]
                ?? throw new InvalidOperationException("ServiceBus:Queue is not configured.");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _processor = _client.CreateProcessor(
                _queueName,
                new ServiceBusProcessorOptions
                {
                    AutoCompleteMessages = false
                });

            _processor.ProcessMessageAsync += async args =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var handler = scope.ServiceProvider.GetRequiredService<LogResponseHandler>();

                    await handler.HandleLogResponse(args);

                    await args.CompleteMessageAsync(args.Message, args.CancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to process SignalR log response. EntityPath: {EntityPath}, MessageId: {MessageId}, CorrelationId: {CorrelationId}",
                        args.EntityPath,
                        args.Message.MessageId,
                        args.Message.CorrelationId);

                    await args.AbandonMessageAsync(
                        args.Message,
                        cancellationToken: args.CancellationToken);
                }
            };

            _processor.ProcessErrorAsync += async args =>
            {
                using var scope = _scopeFactory.CreateScope();

                var handler = scope.ServiceProvider.GetRequiredService<LogResponseHandler>();

                await handler.HandleError(args);
            };

            await _processor.StartProcessingAsync(stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_processor != null)
            {
                await _processor.StopProcessingAsync(cancellationToken);
                await _processor.DisposeAsync();
            }

            await base.StopAsync(cancellationToken);
        }
    }
}