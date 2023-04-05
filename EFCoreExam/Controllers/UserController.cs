using EFCoreExam.Request;
using EFCoreExam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EFCoreExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("{userId}/roles/{roleName}")]
        public async Task<IActionResult> AddRoleToUser(string userId, string roleName)
        {
            var result = await _userService.AddRoleToUserAsync(userId, roleName);
            if(string.IsNullOrEmpty(result))
            {
                return BadRequest(result);
            }
            return Ok(new { result });
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            var user = await _userService.CreateUserAsync(request.Username, request.Email, request.Password, request.RoleName);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
