using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookShelfApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace BookShelfApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user)
        {
            var jwtSection = _configuration.GetSection("Jwt");

            var secretKey = jwtSection["Key"];

            var keyBytes = Encoding.UTF8.GetBytes(secretKey);

            var securityKey = new SymmetricSecurityKey(keyBytes);

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role.RoleName)
        };

            var expireMinutes = int.Parse(jwtSection["ExpireMinutes"] ?? "720");

            var expiration = DateTime.UtcNow.AddMinutes(expireMinutes);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return tokenString;
        }
    }
}
