using blog_api_aspnet_6.Attibutes;
using Microsoft.AspNetCore.Mvc;

namespace blog_api_aspnet_6.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        [ApiKey]
        public IActionResult Get()
        {            
            return Ok();
        }

        [HttpGet("env")]        
        public IActionResult GetEnv([FromServices] IConfiguration configuration)
        {
            var env = configuration.GetValue<string>("Env");
            return Ok(new { env = env });
        }
    }
}
