using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public sealed class EmailOrPhoneAttribute : ValidationAttribute
{
    private static readonly Regex PhoneRegex = new(
        ValidationPatterns.Phone,
        RegexOptions.Compiled
    );

    private static readonly Regex EmailRegex = new(
        ValidationPatterns.Email,
        RegexOptions.Compiled
    );

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is not string input || string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Success;
        }

        return EmailRegex.IsMatch(input) || PhoneRegex.IsMatch(input)
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessageString);
    }
}