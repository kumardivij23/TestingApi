using Microsoft.AspNetCore.Mvc;
using TestingApi.Models;
using TestingApi.Services;

namespace TestingApi.Controllers
{
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

        [HttpPost("calculate")]
        public IActionResult Calculate([FromBody] DiscountRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Calculating discount for amount: {Amount}", request.Amount);

            var response = _discountService.CalculateDiscount(request);

            _logger.LogInformation(
                "Discount calculated — Original: {Original}, Discount: {Discount}%, Amount Off: {AmountOff}, Final: {Final}",
                response.OriginalAmount, response.DiscountPercentage, response.DiscountAmount, response.FinalAmount);

            return Ok(response);
        }
    }
}
