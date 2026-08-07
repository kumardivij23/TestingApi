using Microsoft.AspNetCore.Mvc;
using TestingApi.Models.DTOs;
using TestingApi.Services;

namespace TestingApi.Controllers
{
    /// <summary>
    /// Controller for managing FIT Score operations.
    /// Provides endpoints to regenerate FIT Scores for candidates.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FITScoreController : ControllerBase
    {
        private readonly IBlandAIService _blandAIService;
        private readonly ILogger<FITScoreController> _logger;

        public FITScoreController(IBlandAIService blandAIService, ILogger<FITScoreController> logger)
        {
            _blandAIService = blandAIService;
            _logger = logger;
        }

        /// <summary>
        /// Regenerates the FIT Score for a particular candidate.
        /// This deletes any existing "AI Interview FIT Score" entries for the candidate
        /// and calls GenerateAIFITScoreRequest in BlandAIService to compute a fresh score.
        /// </summary>
        /// <param name="request">The request containing the CandidateApplicationId.</param>
        /// <returns>The regenerated FIT Score details.</returns>
        /// <response code="200">FIT Score successfully regenerated.</response>
        /// <response code="400">Invalid request (e.g., CandidateApplicationId is 0 or negative).</response>
        /// <response code="500">An unexpected error occurred during score regeneration.</response>
        [HttpPost("regenerate")]
        [ProducesResponseType(typeof(RegenerateFITScoreResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RegenerateFITScoreResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RegenerateFITScoreResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegenerateFITScore([FromBody] RegenerateFITScoreRequestDto request)
        {
            _logger.LogInformation(
                "RegenerateFITScore endpoint called for CandidateApplicationId: {CandidateApplicationId}",
                request.CandidateApplicationId);

            // Validate model state
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for RegenerateFITScore request.");
                return BadRequest(new RegenerateFITScoreResponseDto
                {
                    Success = false,
                    Message = "Invalid request. CandidateApplicationId must be greater than 0.",
                    CandidateApplicationId = request.CandidateApplicationId
                });
            }

            try
            {
                // Call the service to regenerate the FIT Score
                var result = await _blandAIService.RegenerateFITScoreAsync(request.CandidateApplicationId);

                if (result == null)
                {
                    _logger.LogWarning(
                        "FIT Score regeneration returned null for CandidateApplicationId: {CandidateApplicationId}",
                        request.CandidateApplicationId);

                    return StatusCode(StatusCodes.Status500InternalServerError, new RegenerateFITScoreResponseDto
                    {
                        Success = false,
                        Message = "Failed to regenerate FIT Score. The scoring service did not return a result.",
                        CandidateApplicationId = request.CandidateApplicationId
                    });
                }

                _logger.LogInformation(
                    "FIT Score successfully regenerated for CandidateApplicationId: {CandidateApplicationId}. New Score: {Score}",
                    request.CandidateApplicationId, result.Score);

                return Ok(new RegenerateFITScoreResponseDto
                {
                    Success = true,
                    Message = "FIT Score successfully regenerated.",
                    CandidateApplicationId = request.CandidateApplicationId,
                    NewScore = result.Score,
                    GeneratedAt = result.CreatedAt
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex,
                    "Validation error during FIT Score regeneration for CandidateApplicationId: {CandidateApplicationId}",
                    request.CandidateApplicationId);

                return BadRequest(new RegenerateFITScoreResponseDto
                {
                    Success = false,
                    Message = ex.Message,
                    CandidateApplicationId = request.CandidateApplicationId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error during FIT Score regeneration for CandidateApplicationId: {CandidateApplicationId}",
                    request.CandidateApplicationId);

                return StatusCode(StatusCodes.Status500InternalServerError, new RegenerateFITScoreResponseDto
                {
                    Success = false,
                    Message = "An unexpected error occurred while regenerating the FIT Score.",
                    CandidateApplicationId = request.CandidateApplicationId
                });
            }
        }
    }
}
