using System.ComponentModel.DataAnnotations;

namespace TestingApi.DTOs
{
    public class RatingRequest
    {
        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "StarValue is required.")]
        [Range(1, 5, ErrorMessage = "StarValue must be between 1 and 5.")]
        public int StarValue { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
    }
}
