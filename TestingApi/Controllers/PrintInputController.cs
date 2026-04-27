using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

        [HttpPost]
        public IActionResult Post([FromBody] JsonElement body)
        {
            _logger.LogInformation("Received POST request with body: {Body}", body.ToString());
            return Ok(body);
        }
    }
}
