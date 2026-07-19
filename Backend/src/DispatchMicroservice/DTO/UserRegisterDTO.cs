using Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace DispatchMicroservice.DTO
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = ResponseCodes.Validation.Username.Required)]
        [MinLength(3, ErrorMessage = ResponseCodes.Validation.Username.TooShort)]
        [MaxLength(24, ErrorMessage = ResponseCodes.Validation.Username.TooLong)]
        public string userName { get; set; } = null!;

        [Required(ErrorMessage = ResponseCodes.Validation.Password.Required)]
        [MinLength(6, ErrorMessage = ResponseCodes.Validation.Password.TooShort)]
        [MaxLength(64, ErrorMessage = ResponseCodes.Validation.Password.TooLong)]
        public string password { get; set; } = null!;
    }
}