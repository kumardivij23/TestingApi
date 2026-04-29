using TestingApi.Models;

namespace TestingApi.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly decimal _discountPercentage;

        public DiscountService(IConfiguration configuration)
        {
            var configuredPercentage = configuration.GetValue<decimal?>("DiscountSettings:DefaultDiscountPercentage");
            _discountPercentage = configuredPercentage ?? 10m;
        }

        public DiscountResponse CalculateDiscount(DiscountRequest request)
        {
            var discountAmount = Math.Round(request.Amount * _discountPercentage / 100m, 2);
            var finalAmount = request.Amount - discountAmount;

            return new DiscountResponse
            {
                OriginalAmount = request.Amount,
                DiscountPercentage = _discountPercentage,
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount
            };
        }
    }
}
