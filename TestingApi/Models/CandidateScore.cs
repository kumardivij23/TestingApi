namespace TestingApi.Models
{
    /// <summary>
    /// Represents a candidate's score record in the system.
    /// </summary>
    public class CandidateScore
    {
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the candidate application.
        /// </summary>
        public int CandidateApplicationId { get; set; }

        /// <summary>
        /// The type of score, e.g. "AI Interview FIT Score".
        /// </summary>
        public string ScoreType { get; set; } = string.Empty;

        /// <summary>
        /// The computed score value.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Optional remarks or details about the score.
        /// </summary>
        public string? Remarks { get; set; }

        /// <summary>
        /// Timestamp when the score was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when the score was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Soft-delete flag.
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
