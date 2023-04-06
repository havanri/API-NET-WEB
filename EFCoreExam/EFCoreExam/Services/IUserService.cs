using EFCoreExam.Data;
using Microsoft.AspNetCore.Identity;

namespace EFCoreExam.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> CreateUserAsync(string username, string email, string password, string roleName, string firstName, string lastName);
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<string> AddRoleToUserAsync(string userId, string roleName);
    }

}
