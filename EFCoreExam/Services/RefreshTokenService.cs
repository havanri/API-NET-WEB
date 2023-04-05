using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EFCoreExam.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        public RefreshTokenService(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        public string GenerateRefreshToken(string userId)
        {
            throw new NotImplementedException();
        }

        public bool ValidateRefreshToken(string userId, string token)
        {
            throw new NotImplementedException();
        }

        /*public string GenerateRefreshToken()
        {
            var refreshTokenHandler = new JwtSecurityTokenHandler();
            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = "http://localhost:5274",
                Audience = "http://localhost:5274",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"])), SecurityAlgorithms.HmacSha256Signature)
            };
            var refreshToken = refreshTokenHandler.CreateToken(refreshTokenDescriptor);
            return refreshTokenHandler.WriteToken(refreshToken);
        }*/

        // Xác thực Refresh Token
        /*public ClaimsPrincipal ValidateRefreshToken(string refreshToken)
        {
            var refreshTokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "http://localhost:5274",
                ValidAudience = "http://localhost:5274",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]))
            };
            try
            {
                var principal = refreshTokenHandler.ValidateToken(refreshToken, validationParameters, out var securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }*/

    }
}
