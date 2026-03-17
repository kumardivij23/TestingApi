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
            _logger.LogInformation("PrintInput GET controller called");
            return "Hello welcome to the controller print";
        }

        [HttpPost]
        public IActionResult Post([FromBody] string input)
        {
            _logger.LogInformation("PrintInput POST controller called with input: {Input}", input);
            
            if (string.IsNullOrEmpty(input))
            {
                _logger.LogWarning("POST request received with null or empty input");
                return BadRequest(new { message = "Input cannot be null or empty" });
            }

            var response = new
            {
                message = "Input received successfully",
                receivedInput = input,
                timestamp = DateTime.UtcNow,
                controller = "PrintInputController"
            };

            _logger.LogInformation("POST request processed successfully for input: {Input}", input);
            return Ok(response);
        }
    }
}