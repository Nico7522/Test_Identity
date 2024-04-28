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
            return Ok("TEST");
        }

        [Authorize(Policy = "ConnectedUser")]
        [HttpGet("TestConnectedUser")]
        public IActionResult Test()
        {
            return Ok("TEST");
        }
    }
}
