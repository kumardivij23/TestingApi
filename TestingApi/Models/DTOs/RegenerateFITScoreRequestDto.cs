using System.ComponentModel.DataAnnotations;

namespace TestingApi.Models.DTOs
{
    /// <summary>
    /// Request DTO for regenerating the FIT Score for a candidate.
    /// </summary>
    public class RegenerateFITScoreRequestDto
    {
        /// <summary>
        /// The unique identifier of the candidate application for which to regenerate the FIT Score.
        /// Must be greater than 0.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "CandidateApplicationId must be greater than 0.")]
        public int CandidateApplicationId { get; set; }
    }
}
