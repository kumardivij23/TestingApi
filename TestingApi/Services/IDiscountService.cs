using TestingApi.Models;

namespace TestingApi.Services
{
    /// <summary>
    /// Interface for discount calculation operations.
    /// </summary>
    public interface IDiscountService
    {
        /// <summary>
        /// Calculates the discounted amount based on the configured discount percentage.
        /// </summary>
        /// <param name="amount">The original amount.</param>
        /// <returns>A DiscountResponse containing the calculation details.</returns>
        DiscountResponse CalculateDiscount(decimal amount);
    }
}
