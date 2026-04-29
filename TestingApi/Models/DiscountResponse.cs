namespace TestingApi.Models
{
    public class DiscountResponse
    {
        public decimal OriginalAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
    }
}
