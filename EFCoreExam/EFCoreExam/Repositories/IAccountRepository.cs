using EFCoreExam.DTOs.Account;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EFCoreExam.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpSync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
        public Task<string> GenerateRefreshToken();
        public Task<ClaimsPrincipal>? GetPrincipalFromExpiredToken(string? token);
        public Task<JwtSecurityToken> CreateToken(List<Claim> authClaims);
    }
}
