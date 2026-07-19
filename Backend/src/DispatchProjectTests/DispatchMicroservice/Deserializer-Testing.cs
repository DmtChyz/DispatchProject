using Xunit;
using System;
using System.Text.Json;
using System.Text.Json.Serialization; // Required for JsonStringEnumConverter
using DispatchProject.Enumerable.ActionLog;
using System.Globalization;
using Shared.DTO.Azure;

namespace DispatchProjectTests.DispatchMicroservice
{
    public class Deserializer_Testing
    {
        // Define JsonSerializerOptions once for the test.
        // This is needed for System.Text.Json to correctly read "Completed" or "Failed" strings.
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };

        [Theory]
        [InlineData(@"
        {
            ""CorrelationId"":""single-test"",
            ""CreatedAt"":""2023-11-15T10:00:00Z"",
            ""Status"":""Completed""
        }",
        "single-test", "2023-11-15T10:00:00Z", ActionStatus.Completed)]
        [InlineData(@"
        {
            ""CorrelationId"":""single-test"",
            ""CreatedAt"":""2023-11-15T10:00:00Z"",
            ""Status"":""Failed""
        }",
        "single-test", "2023-11-15T10:00:00Z", ActionStatus.Failed)]
        public void Deserializes_LogResponse_OneSpecificCase(
            string json,
            string expectedCorrelationId,
            string expectedCreatedAtString,
            ActionStatus expectedStatus)
        {
            LogResponse result = JsonSerializer.Deserialize<LogResponse>(json, _jsonSerializerOptions);

            // Assertions
            Assert.NotNull(result);
            Assert.Equal(expectedCorrelationId, result.CorrelationId);

            // Convert expected date string to DateTime for comparison
            DateTime expectedCreatedAt = DateTime.Parse(expectedCreatedAtString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            Assert.Equal(expectedCreatedAt, result.CreatedAt);

            Assert.Equal(expectedStatus, result.Status);
        }
    }
}