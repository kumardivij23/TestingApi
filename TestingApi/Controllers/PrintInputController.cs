using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace TestingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrintInputController : ControllerBase
    {
        private readonly ILogger<PrintInputController> _logger;

        public PrintInputController(ILogger<PrintInputController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            _logger.LogInformation("PrintInput controller called");
            return "Hello welcome to the controller print";
        }

        [HttpPost("echo")]
        public IActionResult Echo([FromBody] JsonElement body)
        {
            _logger.LogInformation("Echo endpoint called with body: {Body}", body.GetRawText());
            return Ok(body);
        }
    }
}
