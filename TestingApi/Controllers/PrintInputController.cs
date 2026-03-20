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
            _logger.LogInformation("PrintInput controller GET method called");
            return "Hello welcome to the controller print";
        }

        [HttpPost]
        public IActionResult Post([FromBody] string input)
        {
            _logger.LogInformation("PrintInput controller POST method called with input: {Input}", input);
            
            if (string.IsNullOrWhiteSpace(input))
            {
                _logger.LogWarning("POST method called with null or empty input");
                return BadRequest(new { error = "Input cannot be null or empty" });
            }

            var response = new
            {
                message = "Input received successfully",
                receivedInput = input,
                timestamp = DateTime.UtcNow,
                controller = "PrintInputController"
            };

            return Ok(response);
        }
    }
}