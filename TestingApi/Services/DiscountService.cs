using Microsoft.Extensions.Configuration;
using TestingApi.Models;

namespace TestingApi.Services
{
    /// <summary>
    /// Service that calculates discounted amounts based on a configurable discount percentage.
    /// </summary>
    public class DiscountService : IDiscountService
    {
        private readonly decimal _discountPercentage;
        private readonly ILogger<DiscountService> _logger;

        /// <summary>
        /// Initializes the DiscountService with configuration and logging.
        /// Reads the discount percentage from appsettings.json under "DiscountSettings:DefaultPercentage".
        /// Defaults to 10% if not configured.
        /// </summary>
        public DiscountService(IConfiguration configuration, ILogger<DiscountService> logger)
        {
            _logger = logger;

            var configuredPercentage = configuration.GetValue<decimal?>("DiscountSettings:DefaultPercentage");
            _discountPercentage = configuredPercentage ?? 10m;

            _logger.LogInformation("DiscountService initialized with discount percentage: {Percentage}%", _discountPercentage);
        }

        /// <inheritdoc />
        public DiscountResponse CalculateDiscount(decimal amount)
        {
            _logger.LogInformation("Calculating discount for amount: {Amount}", amount);

            var discountAmount = Math.Round(amount * (_discountPercentage / 100m), 2);
            var finalAmount = amount - discountAmount;

            var response = new DiscountResponse
            {
                OriginalAmount = amount,
                DiscountPercentage = _discountPercentage,
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount
            };

            _logger.LogInformation(
                "Discount calculated — Original: {Original}, Discount: {Discount}, Final: {Final}",
                response.OriginalAmount, response.DiscountAmount, response.FinalAmount);

            return response;
        }
    }
}
