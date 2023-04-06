using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;

namespace EFCoreExam.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> AddClaimToRole(int roleId, string claimType, string claimValue)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return false;
            }

            var claim = new Claim(claimType, claimValue);

            var result = await _roleManager.AddClaimAsync(role, claim);
            return result.Succeeded;
        }
        public async Task<IdentityRole> CreateRole(string roleName)
        {
            var role = new IdentityRole { Name = roleName };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return role;
            }
            else
            {
                return null;
            }
        }
    }
}
