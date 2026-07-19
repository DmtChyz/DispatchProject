using DispatchProject.Enumerable.ActionLog;

namespace DispatchMicroservice.Validation.Validator
{
    public class RecipientValidator
    {
        private readonly Dictionary<ActionType, Func<string, bool>> _rules;
        // Represent validation rules that will be performed for certain ActionType to defined Validation rule Func<string, bool>
        public RecipientValidator(Validator validator)
        {
            _rules = new Dictionary<ActionType, Func<string, bool>>
            {
                { ActionType.SendEmail, validator.IsEmail },
                { ActionType.SendSms,   validator.IsPhone }
            };
        }
        /// <summary>
        /// Validate recipient data by Validator methods (number or email) according to passed ActionType
        /// </summary>
        /// <param name="action"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        public bool IsValidRecipient(ActionType action, string recipient)
        {
            return _rules.TryGetValue(action, out var validationFunc) && validationFunc(recipient);
        }
    }
}
