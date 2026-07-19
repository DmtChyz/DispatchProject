using DispatchMicroservice.DTO;
using DispatchMicroservice.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.DTO.Contracts.ApiResponses;
using System.Security.Claims;

namespace DispatchMicroservice.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IWebHostEnvironment _environment;

        public UserController(UserService userService, IWebHostEnvironment environment)
        {
            _userService = userService;
            _environment = environment;
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult<ApiResponse<AuthResponse>> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrWhiteSpace(userId) ||
                string.IsNullOrWhiteSpace(userName))
            {
                return Unauthorized(
                    ApiResponse<AuthResponse>.Fail(
                        ResponseCodes.Auth.Unauthorized
                    )
                );
            }

            return Ok(
                ApiResponse<AuthResponse>.Ok(
                    ResponseCodes.Auth.CurrentUserSuccess,
                    new AuthResponse(
                        new AuthUserDto(userId, userName)
                    )
                )
            );
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> UserRegister(
            [FromBody] UserRegisterDTO userData)
        {
            var result = await _userService.RegisterUser(
                userData.userName,
                userData.password
            );

            if (!result.Success)
            {
                return MapFailure(
                    result.Errors,
                    StatusCodes.Status400BadRequest
                );
            }

            SetAuthCookie(result.Value!.Token);

            return Ok(ApiResponse<AuthResponse>.Ok(
                ResponseCodes.Auth.RegisterSuccess,
                new AuthResponse(result.Value.User)
            ));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> UserLogin(
            [FromBody] UserRegisterDTO userData)
        {
            var result = await _userService.LoginUser(
                userData.userName,
                userData.password
            );

            if (!result.Success)
            {
                return MapFailure(
                    result.Errors,
                    StatusCodes.Status401Unauthorized
                );
            }

            SetAuthCookie(result.Value!.Token);

            return Ok(ApiResponse<AuthResponse>.Ok(
                ResponseCodes.Auth.LoginSuccess,
                new AuthResponse(result.Value.User)
            ));
        }

        [Authorize]
        [HttpPost("logout")]
        public ActionResult<ApiResponse> Logout()
        {
            DeleteAuthCookie();
            return Ok(ApiResponse.Ok(ResponseCodes.Auth.LogoutSuccess));
        }

        private ActionResult<ApiResponse<AuthResponse>> MapFailure(
            string errorCode,
            int defaultStatusCode)
        {
            var statusCode = errorCode == ResponseCodes.System.UnexpectedError
                ? StatusCodes.Status500InternalServerError
                : defaultStatusCode;

            return StatusCode(
                statusCode,
                ApiResponse<AuthResponse>.Fail(errorCode)
            );
        }

        private void SetAuthCookie(string token)
        {
            var options = CreateAuthCookieOptions();

            options.Expires = DateTimeOffset.UtcNow.Add(
                AuthCookieConstants.Lifetime
            );

            Response.Cookies.Append(
                AuthCookieConstants.Name,
                token,
                options
            );
        }

        private void DeleteAuthCookie()
        {
            Response.Cookies.Delete(
                AuthCookieConstants.Name,
                CreateAuthCookieOptions()
            );
        }

        private CookieOptions CreateAuthCookieOptions()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Path = "/"
            };
        }
    }
}