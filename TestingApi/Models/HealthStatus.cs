namespace TestingApi.Models
{
    public class HealthStatus
    {
        public string Status { get; set; } = string.Empty;

        public DateTime CheckedAt { get; set; }

        public string Version { get; set; } = string.Empty;
    }
}
