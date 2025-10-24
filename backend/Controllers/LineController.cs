using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LineController : ControllerBase
    {
        private readonly ILogger<LineController> _logger;

        public LineController(ILogger<LineController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult AmIOk()
        {
            _logger.LogInformation("Heart beat received.");
            return Ok();
        }
    }
}
