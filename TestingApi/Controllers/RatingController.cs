using Microsoft.AspNetCore.Mvc;
using TestingApi.DTOs;
using TestingApi.Models;

namespace TestingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        private static readonly List<Rating> _ratings = new();
        private static int _nextId = 1;
        private static readonly object _lock = new();

        private readonly ILogger<RatingController> _logger;

        public RatingController(ILogger<RatingController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get all ratings.
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<Rating>> GetAll()
        {
            _logger.LogInformation("Getting all ratings");
            lock (_lock)
            {
                return Ok(_ratings.ToList());
            }
        }

        /// <summary>
        /// Get a rating by ID.
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Rating> GetById(int id)
        {
            _logger.LogInformation("Getting rating with ID {Id}", id);
            lock (_lock)
            {
                var rating = _ratings.FirstOrDefault(r => r.Id == id);
                if (rating == null)
                {
                    return NotFound(new { message = $"Rating with ID {id} not found." });
                }
                return Ok(rating);
            }
        }

        /// <summary>
        /// Create a new rating.
        /// </summary>
        [HttpPost]
        public ActionResult<Rating> Create([FromBody] RatingRequest request)
        {
            _logger.LogInformation("Creating a new rating for user {UserId}", request.UserId);
            var now = DateTime.UtcNow;
            lock (_lock)
            {
                var rating = new Rating
                {
                    Id = _nextId++,
                    UserId = request.UserId,
                    StarValue = request.StarValue,
                    Description = request.Description,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                _ratings.Add(rating);
                return CreatedAtAction(nameof(GetById), new { id = rating.Id }, rating);
            }
        }

        /// <summary>
        /// Update an existing rating.
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult<Rating> Update(int id, [FromBody] RatingRequest request)
        {
            _logger.LogInformation("Updating rating with ID {Id}", id);
            lock (_lock)
            {
                var rating = _ratings.FirstOrDefault(r => r.Id == id);
                if (rating == null)
                {
                    return NotFound(new { message = $"Rating with ID {id} not found." });
                }

                rating.UserId = request.UserId;
                rating.StarValue = request.StarValue;
                rating.Description = request.Description;
                rating.UpdatedAt = DateTime.UtcNow;

                return Ok(rating);
            }
        }

        /// <summary>
        /// Delete a rating by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation("Deleting rating with ID {Id}", id);
            lock (_lock)
            {
                var rating = _ratings.FirstOrDefault(r => r.Id == id);
                if (rating == null)
                {
                    return NotFound(new { message = $"Rating with ID {id} not found." });
                }

                _ratings.Remove(rating);
                return NoContent();
            }
        }
    }
}
