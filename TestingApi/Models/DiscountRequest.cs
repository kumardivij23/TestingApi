using System.ComponentModel.DataAnnotations;

namespace TestingApi.Models
{
    public class DiscountRequest
    {
        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
