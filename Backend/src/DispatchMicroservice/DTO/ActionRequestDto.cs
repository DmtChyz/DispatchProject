using DispatchMicroservice.Validation.Attributes;
using DispatchProject.Enumerable.ActionLog;
using Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace DispatchMicroservice.DTO
{
    public class ActionRequestDto
    {
        [Required(ErrorMessage = ResponseCodes.Validation.ActionType.Required)]
        [ActionTypeEmailOrPhone(ErrorMessage = ResponseCodes.Validation.ActionType.Invalid)]
        public ActionType? ActionType { get; set; }

        [Required(ErrorMessage = ResponseCodes.Validation.Recipient.Required)]
        [EmailOrPhone(ErrorMessage = ResponseCodes.Validation.Recipient.Invalid)]
        public string Recipient { get; set; } = string.Empty;

        [Required(ErrorMessage = ResponseCodes.Validation.Details.Required)]
        [MinLength(20, ErrorMessage = ResponseCodes.Validation.Details.TooShort)]
        [MaxLength(256, ErrorMessage = ResponseCodes.Validation.Details.TooLong)]
        public string Details { get; set; } = string.Empty;
    }
}