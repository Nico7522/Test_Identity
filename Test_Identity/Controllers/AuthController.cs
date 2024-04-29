using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Test_Identity.Models;
using Test_Identity.Services;

namespace Test_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(LoginUser user)
        {

            if (await _authService.RegisterUser(user))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string token = await _authService.Login(user);
            if (token is not null)
            {
                return Ok(token);
            }

            return BadRequest();
        }
        [Authorize(Policy = "RoleManagement")]
        [HttpGet("GetRole/{userId}")]
        public async Task<IActionResult> GetRole(string userId)
        {
           Claim? role = await _authService.GetRole(userId);
           return role is not null ? Ok(role) : BadRequest();
        }

        [Authorize(Policy = "RoleManagement")]
        [HttpPost("SetRole")]
        public async Task<IActionResult> SetRole([FromBody] SetRoleUser userRole)
        {
            bool isRoleAdded = await _authService.SetRole(userRole.UserId, userRole.Role);
            return isRoleAdded ? Ok() : BadRequest();
        }

    }
}
