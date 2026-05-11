using System.Collections.Concurrent;
using TestingApi.Models;

namespace TestingApi.Services
{
    /// <summary>
    /// Interface for the rating service.
    /// </summary>
    public interface IRatingService
    {
        Rating CreateRating(CreateRatingRequest request);
        Rating? GetRatingById(Guid id);
        IEnumerable<Rating> GetRatingsByItemId(string itemId);
        RatingSummary? GetRatingSummary(string itemId);
        Rating? UpdateRating(Guid id, UpdateRatingRequest request);
        bool DeleteRating(Guid id);
        Dictionary<int, string> GetStarDescriptions();
    }

    /// <summary>
    /// In-memory rating service backed by a ConcurrentDictionary.
    /// Designed to be registered as a Singleton.
    /// </summary>
    public class RatingService : IRatingService
    {
        private readonly ConcurrentDictionary<Guid, Rating> _ratings = new();

        private static readonly Dictionary<int, string> StarDescriptions = new()
        {
            { 1, "Terrible" },
            { 2, "Poor" },
            { 3, "Average" },
            { 4, "Good" },
            { 5, "Excellent" }
        };

        /// <summary>
        /// Creates a new rating from the request DTO.
        /// </summary>
        public Rating CreateRating(CreateRatingRequest request)
        {
            var rating = new Rating
            {
                Id = Guid.NewGuid(),
                Stars = request.Stars,
                Feedback = request.Feedback,
                ItemId = request.ItemId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _ratings[rating.Id] = rating;
            return rating;
        }

        /// <summary>
        /// Retrieves a rating by its unique ID.
        /// </summary>
        public Rating? GetRatingById(Guid id)
        {
            _ratings.TryGetValue(id, out var rating);
            return rating;
        }

        /// <summary>
        /// Retrieves all ratings for a specific item.
        /// </summary>
        public IEnumerable<Rating> GetRatingsByItemId(string itemId)
        {
            return _ratings.Values
                .Where(r => r.ItemId.Equals(itemId, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        /// <summary>
        /// Returns a summary (average, total, distribution) for a specific item.
        /// </summary>
        public RatingSummary? GetRatingSummary(string itemId)
        {
            var itemRatings = _ratings.Values
                .Where(r => r.ItemId.Equals(itemId, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (itemRatings.Count == 0)
            {
                return new RatingSummary
                {
                    ItemId = itemId,
                    AverageStars = 0,
                    TotalRatings = 0,
                    Distribution = new Dictionary<int, int>
                    {
                        { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }
                    }
                };
            }

            var distribution = new Dictionary<int, int>
            {
                { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }
            };

            foreach (var r in itemRatings)
            {
                distribution[r.Stars]++;
            }

            return new RatingSummary
            {
                ItemId = itemId,
                AverageStars = Math.Round(itemRatings.Average(r => r.Stars), 2),
                TotalRatings = itemRatings.Count,
                Distribution = distribution
            };
        }

        /// <summary>
        /// Updates an existing rating's stars and feedback.
        /// </summary>
        public Rating? UpdateRating(Guid id, UpdateRatingRequest request)
        {
            if (!_ratings.TryGetValue(id, out var existing))
                return null;

            existing.Stars = request.Stars;
            existing.Feedback = request.Feedback;
            existing.UpdatedAt = DateTime.UtcNow;

            _ratings[id] = existing;
            return existing;
        }

        /// <summary>
        /// Deletes a rating by ID.
        /// </summary>
        public bool DeleteRating(Guid id)
        {
            return _ratings.TryRemove(id, out _);
        }

        /// <summary>
        /// Returns human-readable ARIA label descriptions for each star value (1–5).
        /// </summary>
        public Dictionary<int, string> GetStarDescriptions()
        {
            return new Dictionary<int, string>(StarDescriptions);
        }
    }
}
