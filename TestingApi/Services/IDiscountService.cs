using TestingApi.Models;

namespace TestingApi.Services
{
    public interface IDiscountService
    {
        DiscountResponse CalculateDiscount(DiscountRequest request);
    }
}
