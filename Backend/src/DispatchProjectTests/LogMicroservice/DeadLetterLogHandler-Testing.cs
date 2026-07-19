using Azure.Messaging.ServiceBus;
using LogMicroservice.Handlers;
using LogMicroservice.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.DTO.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DispatchProjectTests.LogMicroservice
{
    public class DeadLetterLogHandler_Testing
    {
        [Theory]
        [InlineData("""{"CorrelationId": "abc-123"}""", "abc-123", "test-queue", "MaxDeliveryCountExceeded", "Failed to process")]
        [InlineData("""{"CorrelationId": null}""", null, "test-queue", null, null)]
        [InlineData("""{}""", null, "another-queue", "SomeReason", null)]
        [InlineData("null", null, "another-queue", null, "SomeError")]
        [InlineData("null", null, null, null, null)]
        public void ExtractMetaDataFromMessage_TestingWithDifferentData(
            string body,
            string? expectedCorrelationId,
            string queueName,
            string? reason,
            string? errorDescription)
                {
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(
                body: BinaryData.FromString(body),
                enqueuedTime: DateTimeOffset.UtcNow
            );

            var result = ExtractMetaDataFromMessageEventArgs(message, queueName, reason, errorDescription);

            Assert.Equal(expectedCorrelationId, result.CorrelationId);
            Assert.Equal(queueName, result.QueueName);
            Assert.Equal(reason, result.Reason);
            Assert.Equal(errorDescription, result.ErrorDescription);
            Assert.NotEqual(default, result.EnqueuedAt);
        }

        private DlqLogMessage ExtractMetaDataFromMessageEventArgs(
            ServiceBusReceivedMessage message,
            string queueName,
            string? reason = null,
            string? errorDescription = null)
        {
            var log = new DlqLogMessage();
            var raw = Encoding.UTF8.GetString(message.Body.ToArray());
            log.QueueName = queueName;
            log.Reason = reason;
            log.ErrorDescription = errorDescription;
            log.EnqueuedAt = message.EnqueuedTime.UtcDateTime;

            try
            {
                JsonDocument doc = JsonDocument.Parse(raw);
                log.MessageBody = JsonSerializer.Serialize(doc.RootElement);
                if (doc.RootElement.ValueKind != JsonValueKind.Null)
                    if (doc.RootElement.TryGetProperty("CorrelationId", out var correlationIdElement))
                        log.CorrelationId = correlationIdElement.GetString();
            }
            catch
            {
                log.MessageBody = raw;
                log.CorrelationId = message.CorrelationId;
            }

            return log;
        }
    }
}
