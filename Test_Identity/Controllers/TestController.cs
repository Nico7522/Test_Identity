using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Test_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [Authorize(Policy = "OnlyAdmin")]
        [HttpGet("TestOnlyAdmin")]

        public IActionResult Get()
        {
            return Ok("TEST ONLY ADMIN CLAIM");
        }

        [Authorize(Policy = "ConnectedUser")]
        [HttpGet("TestConnectedUser")]
        public IActionResult Test()
        {
            return Ok("TEST CONNECTED USER CLAIM");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("TestAdminRole")]
        public IActionResult TestAdminRole()
        {
            return Ok("TEST ADMIN ROLE");
        }

        [Authorize(Policy = "FidelityRequirement")]
        [HttpGet("TestPersonalMethod")]
        public IActionResult GetByFidelityProduct()
        {
            return Ok("TEST PERSONAL METHOD");
        }
    }
}
