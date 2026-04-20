namespace TestingApi.Models
{
    /// <summary>
    /// Response model containing the discount calculation result.
    /// </summary>
    public class DiscountResponse
    {
        /// <summary>
        /// The original amount before discount.
        /// </summary>
        public decimal OriginalAmount { get; set; }

        /// <summary>
        /// The discount percentage applied.
        /// </summary>
        public decimal DiscountPercentage { get; set; }

        /// <summary>
        /// The calculated discount amount.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// The final amount after applying the discount.
        /// </summary>
        public decimal FinalAmount { get; set; }
    }
}
