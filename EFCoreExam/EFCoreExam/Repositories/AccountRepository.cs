using EFCoreExam.Data;
using EFCoreExam.DTOs.Account;
using EFCoreExam.Request;
using EFCoreExam.Response;
using EFCoreExam.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EFCoreExam.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ILogger<AccountRepository> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            _logger = logger;
        }
        public async Task<string> SignInAsync(SignInModel model)
        {
            try
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (!result.Succeeded)
                {
                    _logger.LogInformation("Sign in failed for email: {Email}", model.Email);
                    throw new ApplicationException("Invalid credentials");
                }
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.LogWarning("User with email {Email} not found", model.Email);
                    throw new ApplicationException("User not found");
                }
                var roles = await userManager.GetRolesAsync(user);
                Log.Information("User {UserId} signed in successfully.", user.Id);

                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
                foreach (var role in roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                //create token
                var token = await CreateToken(authClaims);
                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                var refreshToken = GenerateRefreshToken();

                _logger.LogDebug("Generated access token: {accesstoken}", accessToken);
                _logger.LogDebug("Generated refresh token: {refreshToken}", refreshToken);
                //add refreshtoken in User
                _ = int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                user.RefreshToken = await refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
                await userManager.UpdateAsync(user);

                return accessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while signing in user with email {Email}", model.Email);
                throw;
            }  
        }

        public async Task<IdentityResult> SignUpSync(SignUpModel model)
        {
            try
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    Log.Information($"User {model.Email} successfully registered."); // Log thông tin
                    return result;
                }
                else
                {
                    Log.Warning($"Failed to register user {model.Email}. Errors: {string.Join(", ", result.Errors)}"); // Log cảnh báo
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while registering user {model.Email}."); // Log lỗi
                throw; // Ném lại lỗi để xử lý ở tầng trên
            }
        }
        public async Task<JwtSecurityToken> CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            _ = int.TryParse(configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            _logger.LogDebug("Time expires: {expires}", tokenValidityInMinutes);
            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
                );
            return token;
        }
        public async Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public async Task<ClaimsPrincipal>? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;

        }
    }
}
