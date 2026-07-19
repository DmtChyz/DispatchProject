using Azure.Messaging.ServiceBus;
using LogMicroservice.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMicroservice.Workers
{
    public class DeadLetterLogWorker : BackgroundService
    {
        private readonly ServiceBusClient _client;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DeadLetterLogWorker> _logger;
        private readonly List<ServiceBusProcessor> _deadLetterProcessors = new();

        public DeadLetterLogWorker(ServiceBusClient client, IServiceScopeFactory scopeFactory, IConfiguration configuration, ILogger<DeadLetterLogWorker> logger)
        {
            _configuration = configuration;
            _client = client;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Dictionary<string, List<string>>? topicAndQueueDictionary = GetTopicQueueDictionary();

            if (topicAndQueueDictionary == null)
            {
                _logger.LogWarning("No Topic/Queue config provided. ");
                throw new InvalidOperationException("No Service Bus DLQ queues/topics configured.");
            }
            // key -> queue/topic name , value -> null or topic subscription
            foreach (var pair in topicAndQueueDictionary)
            {
                if (pair.Value == null)
                {
                    await StartProcessor(_client.CreateProcessor(pair.Key, new ServiceBusProcessorOptions { SubQueue = SubQueue.DeadLetter, AutoCompleteMessages = false }), stoppingToken);

                }
                else
                {
                    foreach (var subscription in pair.Value)
                    {
                        await StartProcessor(_client.CreateProcessor(pair.Key, subscription, new ServiceBusProcessorOptions { SubQueue = SubQueue.DeadLetter, AutoCompleteMessages = false }), stoppingToken);
                    }
                }

            }
            await Task.Delay(Timeout.Infinite, stoppingToken);   // calling when all the proccesors are ready to listen constantly.
        }
        private async Task StartProcessor(ServiceBusProcessor processor, CancellationToken stoppingToken)
        {
            processor.ProcessMessageAsync += async args =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<DeadLetterLogHandler>();
                    await handler.HandleDeadLetterLogSending(args, args.EntityPath);
                    await args.CompleteMessageAsync(args.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to process DLQ message. EntityPath: {EntityPath}, MessageId: {MessageId}, CorrelationId: {CorrelationId}",
                        args.EntityPath,
                        args.Message.MessageId,
                        args.Message.CorrelationId);
                    await args.AbandonMessageAsync(args.Message);
                }
            };
            processor.ProcessErrorAsync += async args =>
            {
                using var scope = _scopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<DeadLetterLogHandler>();
                await handler.HandleError(args);
            };

            _deadLetterProcessors.Add(processor);

            await processor.StartProcessingAsync(stoppingToken); // starting each processor
        }
        private Dictionary<string, List<string>>? GetTopicQueueDictionary()
        {
            var queues = _configuration
                .GetSection("ServiceBus:DeadLetterQueues")
                .Get<List<string>>() ?? new List<string>();

            var topics = _configuration
                .GetSection("ServiceBus:DeadLetterTopics")
                .Get<Dictionary<string, List<string>>>() ?? new Dictionary<string, List<string>>();

            if (queues.Count == 0 && topics.Count == 0)
            {
                return null;
            }
            var TopicQueueDictionary = new Dictionary<string, List<string>>();
            foreach (var queue in queues)
            {
                TopicQueueDictionary.Add(queue, null);
            }
            foreach (var topic in topics)
            {
                TopicQueueDictionary.Add(topic.Key, topic.Value);
            }
            return TopicQueueDictionary;
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach(var processor in _deadLetterProcessors)
            {
                try
                {
                    await processor.StopProcessingAsync(cancellationToken);
                    await processor.DisposeAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to stop/dispose DLQ processor. EntityPath: {EntityPath}",
                        processor.EntityPath);
                }
            }
            await base.StopAsync(cancellationToken);
        }
    }
}
