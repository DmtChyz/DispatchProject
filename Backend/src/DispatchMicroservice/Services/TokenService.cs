using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DispatchMicroservice.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(string username, string id)
        {
            if (username == null || id == null) return null;

            var claimsList = GetClaim(username,id);
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Secret").Value));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenObject = new JwtSecurityToken(

                issuer: _configuration.GetSection("JwtSettings:Issuer").Value,
                audience: _configuration.GetSection("JwtSettings:Audience").Value,
                claims: claimsList,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddDays(15)
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenObject); ;

        }
        public string GenerateInternalAccessToken()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Internal")
            };

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["InternalAuth:Secret"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenObject = new JwtSecurityToken(
                issuer: _configuration["InternalAuth:Issuer"],
                audience: _configuration["InternalAuth:Audience"],
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddSeconds(5)
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenObject);
        }


        protected static List<Claim> GetClaim(string userName, string userId)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Name,userName),
                new Claim(JwtRegisteredClaimNames.Sub,userId),
            };
            return claims;
        }
    }
}
