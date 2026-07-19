using System.Text.RegularExpressions;

namespace DispatchMicroservice.Validation.Validator
{
    public class Validator
    {
        private readonly Regex _phoneRegex;
        private readonly Regex _emailRegex;
        public Validator()
        {
            _phoneRegex = new Regex(ValidationPatterns.Phone, RegexOptions.Compiled);
            _emailRegex = new Regex(ValidationPatterns.Email, RegexOptions.Compiled);
        }
        public bool IsPhone(string phone)
        {
            return _phoneRegex.IsMatch(phone);
        }
        public bool IsEmail(string email)
        {
            return _emailRegex.IsMatch(email);
        }

    }
}
