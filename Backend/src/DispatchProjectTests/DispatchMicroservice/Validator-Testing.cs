using DispatchMicroservice.Validation.Attributes;
using DispatchProject.Enumerable.ActionLog;

namespace DispatchProjectTests.DispatchMicroservice
{
    public class Validator_Testing
    {
        [Theory]
        [InlineData(null, true)]
        [InlineData(ActionType.SendEmail, true)]
        [InlineData(ActionType.SendSms, true)]
        [InlineData((ActionType)999, false)]
        public void AttributeActionTypeEmailOrPhone_WithDifferentData(
        ActionType? data,
        bool expected)
        {
            var attribute = new ActionTypeEmailOrPhoneAttribute();

            var actual = attribute.IsValid(data);

            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData("+38068", false)]
        [InlineData("+3806841343", true)]
        [InlineData("0689380286", false)]
        [InlineData("33@g.com", true)]
        [InlineData("33g.com", false)]
        [InlineData("fdafad@gmail.com", true)]
        [InlineData("3@1.com", false)] // 1 character at start -
        [InlineData("3f@1.com", true)]
        public void AttributeStringEmailOrPhone_WithDifferentData(string data,bool expected)
        {
            var method = new EmailOrPhoneAttribute();
            var actual = method.IsValid(data);
            Assert.Equal(expected,actual);
        }
    }
}
