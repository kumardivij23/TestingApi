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
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation("PrintInput controller called");
                return Ok("Hello welcome to the controller print");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GET /PrintInput");
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] JsonElement body)
        {
            try
            {
                if (body.ValueKind == JsonValueKind.Undefined)
                {
                    _logger.LogWarning("POST /PrintInput received an empty or undefined body");
                    return BadRequest(new { error = "Request body cannot be empty." });
                }

                _logger.LogInformation("Received POST request with body: {Body}", body.ToString());
                return Ok(body);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Invalid JSON in POST /PrintInput");
                return BadRequest(new { error = "Invalid JSON format in request body." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in POST /PrintInput");
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }
    }
}
