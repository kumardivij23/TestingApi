using System.ComponentModel.DataAnnotations;

namespace TestingApi.Models
{
    /// <summary>
    /// Request model for discount calculation.
    /// </summary>
    public class DiscountRequest
    {
        /// <summary>
        /// The original amount on which the discount will be applied.
        /// Must be greater than zero.
        /// </summary>
        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
