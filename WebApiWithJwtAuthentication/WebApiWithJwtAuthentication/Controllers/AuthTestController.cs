using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiWithJwtAuthentication.Controllers
{
    [Route("api/authtest")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles ="User")]
        public IActionResult Get()
        {
            return Ok("You Are Authenticated");
        }
    }
}
