using System.ComponentModel.DataAnnotations;

namespace TestingApi.Models
{
    /// <summary>
    /// Represents a star rating submitted by a user.
    /// </summary>
    public class Rating
    {
        /// <summary>
        /// Unique identifier for the rating.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The star value (1–5).
        /// </summary>
        [Required(ErrorMessage = "Star value is required.")]
        [Range(1, 5, ErrorMessage = "Star value must be between 1 and 5.")]
        public int Stars { get; set; }

        /// <summary>
        /// Optional feedback text from the user.
        /// </summary>
        [MaxLength(1000, ErrorMessage = "Feedback must not exceed 1000 characters.")]
        public string? Feedback { get; set; }

        /// <summary>
        /// Identifier for the item being rated (e.g. product ID, page slug).
        /// </summary>
        [Required(ErrorMessage = "ItemId is required.")]
        [MaxLength(200, ErrorMessage = "ItemId must not exceed 200 characters.")]
        public string ItemId { get; set; } = string.Empty;

        /// <summary>
        /// UTC timestamp when the rating was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UTC timestamp when the rating was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Request DTO for creating a new rating.
    /// </summary>
    public class CreateRatingRequest
    {
        [Required(ErrorMessage = "Star value is required.")]
        [Range(1, 5, ErrorMessage = "Star value must be between 1 and 5.")]
        public int Stars { get; set; }

        [MaxLength(1000, ErrorMessage = "Feedback must not exceed 1000 characters.")]
        public string? Feedback { get; set; }

        [Required(ErrorMessage = "ItemId is required.")]
        [MaxLength(200, ErrorMessage = "ItemId must not exceed 200 characters.")]
        public string ItemId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request DTO for updating an existing rating.
    /// </summary>
    public class UpdateRatingRequest
    {
        [Required(ErrorMessage = "Star value is required.")]
        [Range(1, 5, ErrorMessage = "Star value must be between 1 and 5.")]
        public int Stars { get; set; }

        [MaxLength(1000, ErrorMessage = "Feedback must not exceed 1000 characters.")]
        public string? Feedback { get; set; }
    }

    /// <summary>
    /// Summary of ratings for a specific item.
    /// </summary>
    public class RatingSummary
    {
        public string ItemId { get; set; } = string.Empty;
        public double AverageStars { get; set; }
        public int TotalRatings { get; set; }
        public Dictionary<int, int> Distribution { get; set; } = new();
    }
}
