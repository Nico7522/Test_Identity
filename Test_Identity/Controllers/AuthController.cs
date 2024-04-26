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

            var x = User;
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (await _authService.Login(user))
            {
                string tokenString = _authService.GenerateToken(user);
                return Ok(tokenString);
            }

            return BadRequest();
        }

        public async Task<IActionResult> GetRole(string userId)
        {
           Claim? role = await _authService.GetRole(userId);
           return role is not null ? Ok(role) : BadRequest();
        }

        public async Task<IActionResult> SetRole(string userId, string role)
        {
            bool isRoleAdded = await _authService.SetRole(userId, role);

            return isRoleAdded ? Ok() : BadRequest();
        }

    }
}
