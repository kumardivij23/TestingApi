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

        /// <summary>
        /// Accepts any JSON body and echoes it back in the response.
        /// </summary>
        /// <param name="input">Any valid JSON payload.</param>
        /// <returns>The same JSON payload the caller sent.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] JsonElement input)
        {
            _logger.LogInformation("PrintInput POST called with body: {Body}", input.GetRawText());
            return Ok(input);
        }
    }
}
