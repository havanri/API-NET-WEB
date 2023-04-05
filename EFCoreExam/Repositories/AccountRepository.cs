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
using System.Text;

namespace EFCoreExam.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ILogger<AccountRepository> _logger;
        private readonly IRefreshTokenService _refreshTokenService;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ILogger<AccountRepository> logger, IRefreshTokenService refreshTokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            _logger = logger;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<string> SignInAsync(SignInModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
            {
                _logger.LogInformation("Sign in failed for email: {Email}", model.Email);
                return String.Empty;
            }      
            var user = await userManager.FindByEmailAsync(model.Email);
            var roles = await userManager.GetRolesAsync(user);
            Log.Information("User {UserId} signed in successfully.", user.Id);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]));
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:ValidIssuer"],
                audience: configuration["Jwt:ValidAudience"],
                expires: DateTime.Now.AddMinutes(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256Signature)
            ); // Sinh Refresh Token
/*            var refreshToken = _refreshTokenService.GenerateRefreshToken(user.Id);*/
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            /*return new JwtSecurityTokenHandler().WriteToken(token);*/
            return accessToken;

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
    }
}
