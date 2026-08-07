using System.ComponentModel.DataAnnotations;
using TestingApi.Models;

namespace TestingApi.DTOs
{
    // ──────────────────────────────────────────────
    //  Request DTOs
    // ──────────────────────────────────────────────

    /// <summary>
    /// DTO for creating a new skill.
    /// </summary>
    public class CreateSkillRequest
    {
        [Required]
        public Guid PersonId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public ProficiencyLevel ProficiencyLevel { get; set; } = ProficiencyLevel.Beginner;
    }

    /// <summary>
    /// DTO for updating an existing skill.
    /// </summary>
    public class UpdateSkillRequest
    {
        [MaxLength(200)]
        public string? Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public ProficiencyLevel? ProficiencyLevel { get; set; }
    }

    /// <summary>
    /// DTO for certifying a skill.
    /// </summary>
    public class CertifySkillRequest
    {
        [Required]
        public CertificationStatus CertificationStatus { get; set; }

        /// <summary>
        /// The date certification was granted.
        /// </summary>
        public DateTime? CertifiedDate { get; set; }

        /// <summary>
        /// The date certification expires.
        /// </summary>
        public DateTime? CertificationExpiryDate { get; set; }

        /// <summary>
        /// The certifying authority name.
        /// </summary>
        [MaxLength(300)]
        public string? CertifyingAuthority { get; set; }
    }

    // ──────────────────────────────────────────────
    //  Response DTOs
    // ──────────────────────────────────────────────

    /// <summary>
    /// DTO representing a skill in API responses.
    /// </summary>
    public class SkillResponse
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProficiencyLevel ProficiencyLevel { get; set; }
        public CertificationStatus CertificationStatus { get; set; }
        public DateTime? CertifiedDate { get; set; }
        public DateTime? CertificationExpiryDate { get; set; }
        public string? CertifyingAuthority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO representing an audit log entry in API responses.
    /// </summary>
    public class SkillAuditLogResponse
    {
        public Guid Id { get; set; }
        public Guid SkillId { get; set; }
        public Guid PersonId { get; set; }
        public AuditAction Action { get; set; }
        public string ActionName { get; set; } = string.Empty;
        public string? Details { get; set; }
        public string? PreviousValues { get; set; }
        public string? NewValues { get; set; }
        public DateTime Timestamp { get; set; }
    }

    // ──────────────────────────────────────────────
    //  Pagination
    // ──────────────────────────────────────────────

    /// <summary>
    /// Generic paginated response wrapper.
    /// </summary>
    /// <typeparam name="T">The type of items in the page.</typeparam>
    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;
    }
}
