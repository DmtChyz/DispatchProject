using DispatchProject.Enumerable.ActionLog;
using FluentAssertions;
using Shared.DTO.Azure;
using Shared.Helper;

namespace DispatchProjectTests.Shared
{
    public class Helper_Testing
    {
        [Theory]
        [InlineData(
            @"{
                ""ActionType"": ""SendEmail"",
                ""Recipient"": ""boss@company.com"",
                ""Details"": ""System online"",
                ""CorrelationId"": ""134g541f-tfgs134f"",
                ""OwnerId"": ""Owner-99""
                }",
                ActionType.SendEmail,
                "boss@company.com",
                "System online",
                "134g541f-tfgs134f",
                "Owner-99"
            )]
        [InlineData(
            @"{
                ""ActionType"": ""SendEmail"",
                ""Recipient"": ""support@example.com"",
                ""OwnerId"": ""Admin""
            }",
            ActionType.SendEmail,
            "support@example.com",
            null,
            null,
            "Admin"
        )]
        [InlineData(
            @"{
                ""ActionType"": """",
                ""Recipient"": ""mindreader@domain.org"",
                ""Details"": ""Thought broadcast complete"",
                ""CorrelationId"": ""mind-link-42"",
                ""OwnerId"": ""PsychicDept""
            }",
            ActionType.Undefined,
            "mindreader@domain.org",
            "Thought broadcast complete",
            "mind-link-42",
            "PsychicDept"
        )]
        [InlineData(
            @"{
            }",
            ActionType.Undefined,
            null,
            null,
            null,
            null
        )]
        [InlineData(
            @"{
                ""ActionType"": ""SendEmail"",
                ""Recipient"": ""test@test.com"",
                ""Details"": ""Broken JSON value"",
                ""CorrelationId"": ""corrupt-id"",
            ",
            ActionType.SendEmail,
            "test@test.com",
            "Broken JSON value",
            "corrupt-id",
            null
        )]
        [InlineData(
            @"{
                ""ActionType"":""SendSms""
            }",
            ActionType.SendSms,
            null,
            null,
            null,
            null)]
        public void TestFallbackDeserializer_WithDifferentData(
            string json,
            ActionType expectedActionType,
            string expectedRecipient,
            string expectedDetails,
            string expectedCorrelationId,
            string expectedOwnerId)
        {
            var expected = new NotificationMessage
            {
                ActionType = expectedActionType,
                Recipient = expectedRecipient,
                Details = expectedDetails,
                CorrelationId = expectedCorrelationId,
                OwnerId = expectedOwnerId
            };
            var result = Helper.FallbackDeserializer<NotificationMessage>(json);

            result.Should().BeEquivalentTo(expected);
        }
    }
}