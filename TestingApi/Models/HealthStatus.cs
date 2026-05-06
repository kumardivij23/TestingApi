namespace TestingApi.Models
{
    public class HealthStatus
    {
        public string? Status { get; set; }

        public DateTime CheckedAt { get; set; }

        public string? Version { get; set; }
    }
}
