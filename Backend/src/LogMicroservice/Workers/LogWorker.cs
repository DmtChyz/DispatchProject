using Azure.Messaging.ServiceBus;
using LogMicroservice.Handlers;
namespace LogMicroservice.Workers
{
    public class LogWorker : BackgroundService
    {
        private readonly ServiceBusClient _client;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        private readonly string _queueName;
        public LogWorker(ServiceBusClient client, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _client = client;
            _scopeFactory = scopeFactory;
            _queueName = _configuration["ServiceBus:Queue"]!;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var processor = _client.CreateProcessor(
                _queueName,
                new ServiceBusProcessorOptions
                {
                    AutoCompleteMessages = false
                });
            processor.ProcessMessageAsync += async args =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<LogHandler>();
                    await handler.HandleLogSending(args);
                }
                catch
                {
                    await args.AbandonMessageAsync(args.Message);
                }
            };
            processor.ProcessErrorAsync += async args =>
            {
                using var scope = _scopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<LogHandler>();
                await handler.HandleError(args);
            };
            await processor.StartProcessingAsync(stoppingToken); // runs on azure, and handle method either ProcessMessage or ProcessError will be return to the refered methods
            await Task.Delay(Timeout.Infinite, stoppingToken);   // called once when program starts, and prevent from stopping, so it start once per program lifetime and never ends.
        }
    }
}
