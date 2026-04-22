using TestingApi.Models;

namespace TestingApi.Services
{
    /// <summary>
    /// Interface for the Bland AI Service that handles FIT Score generation and regeneration.
    /// </summary>
    public interface IBlandAIService
    {
        /// <summary>
        /// Generates an AI FIT Score for a given candidate application.
        /// Calls the ML API to compute the FIT Score based on interview data.
        /// </summary>
        /// <param name="candidateApplicationId">The unique identifier of the candidate application.</param>
        /// <returns>The generated CandidateScore, or null if generation failed.</returns>
        Task<CandidateScore?> GenerateAIFITScoreRequestAsync(int candidateApplicationId);

        /// <summary>
        /// Regenerates the FIT Score for a particular candidate by:
        /// 1. Deleting existing entries in CandidateScore with ScoreType "AI Interview FIT Score"
        /// 2. Calling GenerateAIFITScoreRequestAsync to compute a fresh score
        /// </summary>
        /// <param name="candidateApplicationId">The unique identifier of the candidate application.</param>
        /// <returns>The newly generated CandidateScore, or null if regeneration failed.</returns>
        Task<CandidateScore?> RegenerateFITScoreAsync(int candidateApplicationId);
    }
}
