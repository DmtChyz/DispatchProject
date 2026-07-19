using DispatchProject.Enumerable.ActionLog;
using DispatchMicroservice.Validation.Validator;

namespace DispatchProjectTests.DispatchMicroservice
{
    public class RecipientValidator_Testing
    {
        private readonly Validator _validator;
        public RecipientValidator_Testing()
        {
            _validator = new Validator();
        }

        [Theory]
        [InlineData(ActionType.SendEmail, "fdaf@gm.co", true)]
        [InlineData(ActionType.SendEmail, "fdaf@.co", false)]
        [InlineData(ActionType.SendEmail, "11@gm.co", true)]
        [InlineData(ActionType.SendEmail, "fdaf@g", false)]
        [InlineData(ActionType.SendEmail, "+341894138", false)]
        public void RecipientValidatior_WithSendEmail(ActionType type, string recipient, bool expected)
        {
            var method = new RecipientValidator(_validator);
            var result = method.IsValidRecipient(type,recipient);
            Assert.True(result == expected);

        }
        [Theory]
        [InlineData(ActionType.SendSms, "+380", false)]
        [InlineData(ActionType.SendSms, "+38068", false)]
        [InlineData(ActionType.SendSms, "096851", false)]
        [InlineData(ActionType.SendSms, "+", false)]
        [InlineData(ActionType.SendSms, "05", false)]
        [InlineData(ActionType.SendSms, "fadfdad", false)]
        public void RecipientValidatior_WithSendSms(ActionType type, string recipient, bool expected)
        {
            var method = new RecipientValidator(_validator);
            var result = method.IsValidRecipient(type, recipient);
            Assert.True(result == expected);
        }
    }
}
