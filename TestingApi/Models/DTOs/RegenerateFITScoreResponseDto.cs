namespace TestingApi.Models.DTOs
{
    /// <summary>
    /// Response DTO returned after regenerating the FIT Score.
    /// </summary>
    public class RegenerateFITScoreResponseDto
    {
        /// <summary>
        /// Indicates whether the regeneration was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// A human-readable message about the operation result.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// The candidate application ID for which the score was regenerated.
        /// </summary>
        public int CandidateApplicationId { get; set; }

        /// <summary>
        /// The newly generated FIT Score (null if regeneration failed).
        /// </summary>
        public double? NewScore { get; set; }

        /// <summary>
        /// Timestamp when the new score was generated.
        /// </summary>
        public DateTime? GeneratedAt { get; set; }
    }
}
