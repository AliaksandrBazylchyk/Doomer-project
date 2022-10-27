using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivateController : ControllerBase
    {
        [Authorize]
        [HttpGet("auth/request")]
        public async Task<IActionResult> PrivateRequest()
        {
            return Ok("You are authorize");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("auth/role/request/admin")]
        public async Task<IActionResult> AdminRequest()
        {
            return Ok("You are admin!");
        }
    }
}
