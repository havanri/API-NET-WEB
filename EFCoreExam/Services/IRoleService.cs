using Microsoft.AspNetCore.Identity;
using System.Data;

namespace EFCoreExam.Services
{
    public interface IRoleService 
    {
        Task<bool> AddClaimToRole(int roleId, string claimType, string claimValue);
        Task<IdentityRole> CreateRole(string roleName);
    }
}
