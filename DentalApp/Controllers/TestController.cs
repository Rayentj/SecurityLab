using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminArea()
        {
            return Ok("✅ Welcome Admin! You have access.");
        }

        [Authorize(Roles = "User")]
        [HttpGet("user-only")]
        public IActionResult UserArea()
        {
            return Ok("✅ Welcome User!");
        }

        [Authorize]
        [HttpGet("any-authenticated")]
        public IActionResult AuthenticatedOnly()
        {
            return Ok("✅ Hello authenticated user.");
        }
    }
}
