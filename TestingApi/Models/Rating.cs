using System.ComponentModel.DataAnnotations;

namespace TestingApi.Models
{
    public class Rating
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        [Range(1, 5)]
        public int StarValue { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
