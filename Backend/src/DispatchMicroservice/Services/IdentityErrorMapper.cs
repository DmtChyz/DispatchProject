using Microsoft.AspNetCore.Identity;
using Shared.Constants;

namespace DispatchMicroservice.Services
{
    public static class IdentityErrorMapper
    {
        public static string[] Map(IEnumerable<IdentityError> errors)
        {
            return errors
                .Select(MapError)
                .Distinct()
                .ToArray();
        }

        private static string MapError(IdentityError error)
        {
            return error.Code switch
            {
                nameof(IdentityErrorDescriber.DuplicateUserName) =>
                    ResponseCodes.Validation.Username.AlreadyExists,

                nameof(IdentityErrorDescriber.InvalidUserName) =>
                    ResponseCodes.Validation.Username.Invalid,

                nameof(IdentityErrorDescriber.PasswordTooShort) =>
                    ResponseCodes.Validation.Password.TooShort,

                nameof(IdentityErrorDescriber.PasswordRequiresDigit) or
                nameof(IdentityErrorDescriber.PasswordRequiresLower) or
                nameof(IdentityErrorDescriber.PasswordRequiresUpper) or
                nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric) or
                nameof(IdentityErrorDescriber.PasswordRequiresUniqueChars) =>
                    ResponseCodes.Validation.Password.Weak,

                _ => ResponseCodes.Validation.IdentityFailed
            };
        }
    }
}