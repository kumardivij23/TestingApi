using System.ComponentModel.DataAnnotations;

namespace TestingApi.Models
{
    /// <summary>
    /// Represents an audit log entry for tracking changes to skills.
    /// </summary>
    public class SkillAuditLog
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The ID of the skill that was changed.
        /// </summary>
        [Required]
        public Guid SkillId { get; set; }

        /// <summary>
        /// The ID of the person who owns the skill.
        /// </summary>
        [Required]
        public Guid PersonId { get; set; }

        /// <summary>
        /// The type of action performed.
        /// </summary>
        public AuditAction Action { get; set; }

        /// <summary>
        /// A human-readable description of the change.
        /// </summary>
        [MaxLength(2000)]
        public string? Details { get; set; }

        /// <summary>
        /// Snapshot of the previous values (JSON serialized) before the change.
        /// </summary>
        public string? PreviousValues { get; set; }

        /// <summary>
        /// Snapshot of the new values (JSON serialized) after the change.
        /// </summary>
        public string? NewValues { get; set; }

        /// <summary>
        /// The date and time when the audit entry was created.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
