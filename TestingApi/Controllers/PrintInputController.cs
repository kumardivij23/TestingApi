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
            _logger.LogInformation("PrintInput controller GET called");
            return "Hello welcome to the controller print";
        }

        [HttpPost]
        public IActionResult Post([FromBody] string input)
        {
            _logger.LogInformation("PrintInput controller POST called with input: {Input}", input);
            
            if (string.IsNullOrEmpty(input))
            {
                return BadRequest("Input cannot be null or empty");
            }
            
            var response = new
            {
                Message = "Input received successfully",
                ReceivedInput = input,
                Timestamp = DateTime.UtcNow
            };
            
            return Ok(response);
        }
    }
}