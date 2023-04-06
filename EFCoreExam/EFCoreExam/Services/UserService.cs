using EFCoreExam.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EFCoreExam.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration configuration;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            this.configuration = configuration;
        }

        public async Task<ApplicationUser> CreateUserAsync(string username, string email, string password, string roleName, string firstName, string lastName)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName
                };

                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new ApplicationException("Failed to create user.");
                }

                var roleExists = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
                }

                await _userManager.AddToRoleAsync(user, roleName);

                return user;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating user.", ex);
            } 
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }
        public async Task<string> AddRoleToUserAsync(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }

                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Failed to add role to user: " + result.Errors.First().Description);
                }
                // Tạo JWT mới với thông tin về Role đã được cập nhật
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Email, user.Email)
                    };
                foreach (var r in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, r));
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Audience = "http://localhost:5274",
                    Issuer = "http://localhost:5274",
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var newJwt = tokenHandler.WriteToken(token);
                return newJwt;
            }
            catch (ArgumentException ex)
            {
                // Xử lý ngoại lệ ArgumentException
                return "ArgumentException: " + ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                // Xử lý ngoại lệ InvalidOperationException
                return "InvalidOperationException: " + ex.Message;
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ khác
                return "Exception: " + ex.Message;
            }

        }
    }
}
