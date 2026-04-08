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

        /// <summary>
        /// Accepts any JSON payload and echoes it back in the response.
        /// </summary>
        /// <param name="body">Any valid JSON object or value.</param>
        /// <returns>The same JSON payload that was posted.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] JsonElement body)
        {
            _logger.LogInformation("PrintInput POST called with body: {Body}", body.GetRawText());
            return Ok(body);
        }
    }
}
