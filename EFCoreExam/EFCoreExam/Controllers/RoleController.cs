using EFCoreExam.Request;
using EFCoreExam.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("{roleId}/claim")]
        public async Task<IActionResult> AddClaimToRole(int roleId, AddClaimToRoleRequest request)
        {
            var result = await _roleService.AddClaimToRole(roleId, request.Type, request.Value);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to add claim to role.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleRequest request)
        {
            var result = await _roleService.CreateRole(request.Name);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to create role.");
            }
        }
    }
}
