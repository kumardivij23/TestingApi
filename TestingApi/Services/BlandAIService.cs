using System.Collections.Concurrent;
using TestingApi.Models;

namespace TestingApi.Services
{
    /// <summary>
    /// Service responsible for generating and regenerating AI FIT Scores.
    /// Manages candidate scores and interfaces with the ML scoring API.
    /// </summary>
    public class BlandAIService : IBlandAIService
    {
        private readonly ILogger<BlandAIService> _logger;

        // In-memory store simulating the CandidateScore database table.
        // In a real application, this would be replaced with an EF Core DbContext / repository.
        private static readonly ConcurrentDictionary<int, List<CandidateScore>> _candidateScores = new();
        private static int _nextId = 1;

        private const string FITScoreType = "AI Interview FIT Score";

        public BlandAIService(ILogger<BlandAIService> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<CandidateScore?> GenerateAIFITScoreRequestAsync(int candidateApplicationId)
        {
            _logger.LogInformation(
                "GenerateAIFITScoreRequestAsync called for CandidateApplicationId: {CandidateApplicationId}",
                candidateApplicationId);

            try
            {
                // Validate input
                if (candidateApplicationId <= 0)
                {
                    _logger.LogWarning(
                        "Invalid CandidateApplicationId: {CandidateApplicationId}. Must be greater than 0.",
                        candidateApplicationId);
                    return null;
                }

                // Simulate calling the ML API to generate a FIT Score.
                // In a real implementation, this would make an HTTP call to the ML scoring service.
                await Task.Delay(100); // Simulates ML API latency

                var random = new Random();
                var generatedScore = Math.Round(random.NextDouble() * 100, 2);

                var candidateScore = new CandidateScore
                {
                    Id = Interlocked.Increment(ref _nextId),
                    CandidateApplicationId = candidateApplicationId,
                    ScoreType = FITScoreType,
                    Score = generatedScore,
                    Remarks = "FIT Score generated via AI Interview analysis.",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                // Store the generated score
                _candidateScores.AddOrUpdate(
                    candidateApplicationId,
                    new List<CandidateScore> { candidateScore },
                    (key, existingList) =>
                    {
                        existingList.Add(candidateScore);
                        return existingList;
                    });

                _logger.LogInformation(
                    "Successfully generated FIT Score {Score} for CandidateApplicationId: {CandidateApplicationId}",
                    generatedScore, candidateApplicationId);

                return candidateScore;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error generating FIT Score for CandidateApplicationId: {CandidateApplicationId}",
                    candidateApplicationId);
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<CandidateScore?> RegenerateFITScoreAsync(int candidateApplicationId)
        {
            _logger.LogInformation(
                "RegenerateFITScoreAsync called for CandidateApplicationId: {CandidateApplicationId}",
                candidateApplicationId);

            try
            {
                // Step 1: Validate input
                if (candidateApplicationId <= 0)
                {
                    _logger.LogWarning(
                        "Invalid CandidateApplicationId: {CandidateApplicationId}. Must be greater than 0.",
                        candidateApplicationId);
                    throw new ArgumentException(
                        "CandidateApplicationId must be greater than 0.",
                        nameof(candidateApplicationId));
                }

                // Step 2: Delete existing CandidateScore entries with ScoreType "AI Interview FIT Score"
                // In a real application this would query the database:
                //   var existingScores = await _dbContext.CandidateScores
                //       .Where(cs => cs.CandidateApplicationId == candidateApplicationId
                //                 && cs.ScoreType == "AI Interview FIT Score"
                //                 && !cs.IsDeleted)
                //       .ToListAsync();
                //   _dbContext.CandidateScores.RemoveRange(existingScores);
                //   await _dbContext.SaveChangesAsync();

                int deletedCount = 0;

                if (_candidateScores.TryGetValue(candidateApplicationId, out var existingScores))
                {
                    var scoresToDelete = existingScores
                        .Where(cs => cs.ScoreType == FITScoreType && !cs.IsDeleted)
                        .ToList();

                    deletedCount = scoresToDelete.Count;

                    foreach (var score in scoresToDelete)
                    {
                        existingScores.Remove(score);
                    }

                    _logger.LogInformation(
                        "Deleted {DeletedCount} existing FIT Score entries for CandidateApplicationId: {CandidateApplicationId}",
                        deletedCount, candidateApplicationId);
                }
                else
                {
                    _logger.LogInformation(
                        "No existing FIT Score entries found for CandidateApplicationId: {CandidateApplicationId}. Proceeding with fresh generation.",
                        candidateApplicationId);
                }

                // Step 3: Call GenerateAIFITScoreRequestAsync to generate a fresh FIT Score
                var newScore = await GenerateAIFITScoreRequestAsync(candidateApplicationId);

                if (newScore == null)
                {
                    _logger.LogError(
                        "Failed to regenerate FIT Score for CandidateApplicationId: {CandidateApplicationId}. " +
                        "GenerateAIFITScoreRequestAsync returned null.",
                        candidateApplicationId);
                    return null;
                }

                _logger.LogInformation(
                    "Successfully regenerated FIT Score for CandidateApplicationId: {CandidateApplicationId}. " +
                    "Deleted {DeletedCount} old entries, new score: {NewScore}",
                    candidateApplicationId, deletedCount, newScore.Score);

                return newScore;
            }
            catch (ArgumentException)
            {
                throw; // Re-throw validation exceptions
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error while regenerating FIT Score for CandidateApplicationId: {CandidateApplicationId}",
                    candidateApplicationId);
                throw;
            }
        }
    }
}
