using Microsoft.AspNetCore.Mvc;
using TestingApi.Models;
using TestingApi.Services;

namespace TestingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly ILogger<RatingController> _logger;

        public RatingController(IRatingService ratingService, ILogger<RatingController> logger)
        {
            _ratingService = ratingService;
            _logger = logger;
        }

        /// <summary>
        /// Submit a new star rating.
        /// </summary>
        /// <param name="request">The rating details.</param>
        /// <returns>The created rating.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Rating), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateRating([FromBody] CreateRatingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Creating rating for item {ItemId} with {Stars} stars", request.ItemId, request.Stars);

            var rating = _ratingService.CreateRating(request);
            return CreatedAtAction(nameof(GetRatingById), new { id = rating.Id }, rating);
        }

        /// <summary>
        /// Get a specific rating by its ID.
        /// </summary>
        /// <param name="id">The rating ID.</param>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Rating), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetRatingById(Guid id)
        {
            var rating = _ratingService.GetRatingById(id);
            if (rating == null)
                return NotFound(new { message = $"Rating with ID '{id}' not found." });

            return Ok(rating);
        }

        /// <summary>
        /// Get all ratings for a specific item.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        [HttpGet("item/{itemId}")]
        [ProducesResponseType(typeof(IEnumerable<Rating>), StatusCodes.Status200OK)]
        public IActionResult GetRatingsByItemId(string itemId)
        {
            var ratings = _ratingService.GetRatingsByItemId(itemId);
            return Ok(ratings);
        }

        /// <summary>
        /// Get rating summary (average, total, distribution) for a specific item.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        [HttpGet("item/{itemId}/summary")]
        [ProducesResponseType(typeof(RatingSummary), StatusCodes.Status200OK)]
        public IActionResult GetRatingSummary(string itemId)
        {
            var summary = _ratingService.GetRatingSummary(itemId);
            return Ok(summary);
        }

        /// <summary>
        /// Update an existing rating.
        /// </summary>
        /// <param name="id">The rating ID.</param>
        /// <param name="request">Updated rating details.</param>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Rating), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateRating(Guid id, [FromBody] UpdateRatingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Updating rating {Id} to {Stars} stars", id, request.Stars);

            var updated = _ratingService.UpdateRating(id, request);
            if (updated == null)
                return NotFound(new { message = $"Rating with ID '{id}' not found." });

            return Ok(updated);
        }

        /// <summary>
        /// Get human-readable descriptions for each star value (1–5), suitable for ARIA labels.
        /// </summary>
        [HttpGet("descriptions")]
        [ProducesResponseType(typeof(Dictionary<int, string>), StatusCodes.Status200OK)]
        public IActionResult GetStarDescriptions()
        {
            var descriptions = _ratingService.GetStarDescriptions();
            return Ok(descriptions);
        }
    }
}
