using Microsoft.AspNetCore.Identity;
using Shared.Constants;
using Shared.DTO.Contracts.ApiResponses;

namespace DispatchMicroservice.Services
{
    public class UserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<IdentityUser> userManager, TokenService tokenService, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<Result<AuthenticatedUserResult>> RegisterUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return Result<AuthenticatedUserResult>.IsFailure(
                    ResponseCodes.Validation.InvalidRequest
                );
            }

            var normalizedUsername = username.Trim();

            var identityUser = new IdentityUser
            {
                UserName = normalizedUsername
            };

            var createResult = await _userManager.CreateAsync(identityUser, password);

            if (!createResult.Succeeded)
            {
                _logger.LogWarning(
                    "User registration failed. Identity errors: {Errors}",
                    string.Join(", ", createResult.Errors.Select(error =>
                        $"{error.Code}: {error.Description}"))
                );

                return Result<AuthenticatedUserResult>.IsFailure(
                    ResponseCodes.Validation.IdentityFailed
                );
            }

            var token = _tokenService.GenerateToken(identityUser.UserName!, identityUser.Id);

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogError(
                    "Token generation failed after registering user {UserId}",
                    identityUser.Id
                );

                return Result<AuthenticatedUserResult>.IsFailure(
                    ResponseCodes.System.UnexpectedError
                );
            }

            var authResult = new AuthenticatedUserResult(
                token,
                new AuthUserDto(
                    identityUser.Id,
                    identityUser.UserName!
                )
            );

            return Result<AuthenticatedUserResult>.IsSuccess(authResult);
        }

        public async Task<Result<AuthenticatedUserResult>> LoginUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return Result<AuthenticatedUserResult>.IsFailure(
                    ResponseCodes.Auth.InvalidCredentials
                );
            }

            var normalizedUsername = username.Trim();

            var user = await _userManager.FindByNameAsync(normalizedUsername);

            if (user == null)
            {
                return Result<AuthenticatedUserResult>.IsFailure(
                    ResponseCodes.Auth.InvalidCredentials
                );
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordValid)
            {
                return Result<AuthenticatedUserResult>.IsFailure(
                    ResponseCodes.Auth.InvalidCredentials
                );
            }

            var token = _tokenService.GenerateToken(user.UserName!, user.Id);

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogError(
                    "Token generation failed while logging in user {UserId}",
                    user.Id
                );

                return Result<AuthenticatedUserResult>.IsFailure(
                    ResponseCodes.System.UnexpectedError
                );
            }

            var authResult = new AuthenticatedUserResult(
                token,
                new AuthUserDto(
                    user.Id,
                    user.UserName!
                )
            );

            return Result<AuthenticatedUserResult>.IsSuccess(authResult);
        }
    }
}