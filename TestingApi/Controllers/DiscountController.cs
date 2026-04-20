using Microsoft.AspNetCore.Mvc;
using TestingApi.Models;
using TestingApi.Services;

namespace TestingApi.Controllers
{
    /// <summary>
    /// API controller for discount calculation operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(IDiscountService discountService, ILogger<DiscountController> logger)
        {
            _discountService = discountService;
            _logger = logger;
        }

        /// <summary>
        /// Calculates the discounted amount for the given input amount.
        /// </summary>
        /// <param name="request">The discount request containing the original amount.</param>
        /// <returns>A DiscountResponse with the original amount, discount percentage, discount amount, and final amount.</returns>
        /// <response code="200">Returns the discount calculation result.</response>
        /// <response code="400">If the request is invalid (e.g., amount is zero or negative).</response>
        [HttpPost("calculate")]
        [ProducesResponseType(typeof(DiscountResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Calculate([FromBody] DiscountRequest request)
        {
            _logger.LogInformation("POST /api/discount/calculate called with amount: {Amount}", request.Amount);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request received for discount calculation.");
                return BadRequest(ModelState);
            }

            var result = _discountService.CalculateDiscount(request.Amount);

            return Ok(result);
        }
    }
}
