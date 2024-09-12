using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        public SecurityController()
        {
        }

        // implement the login method
        [HttpPost("login")]
        public IActionResult Login()
        {
            // implement service logic here

            return Ok();
        }
    }
}
