using DispatchProject.Enumerable.ActionLog;
using Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace DispatchMicroservice.Validation.Attributes;

public sealed class ActionTypeEmailOrPhoneAttribute : ValidationAttribute
{
    public ActionTypeEmailOrPhoneAttribute()
    {
        ErrorMessage = ResponseCodes.Validation.ActionType.Invalid;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is ActionType.SendEmail or ActionType.SendSms)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessageString);
    }
}