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
        [HttpPost("auth/request")]
        public async Task<IActionResult> PrivateRequest()
        {
            return Ok("You are authorize");
        }
    }
}
