using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingApi.Models
{
    /// <summary>
    /// Unified CCOG Skill entity representing a consolidated skill record
    /// across all sources (DOS taxonomy, employee self-report, manager validation).
    /// </summary>
    [Table("CCOG_Skills")]
    public class CCOG_Skill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Name of the skill (e.g. "C#", "Project Management", "AWS Lambda").
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string SkillName { get; set; } = string.Empty;

        /// <summary>
        /// Category grouping (e.g. "Programming Language", "Cloud", "Soft Skill").
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Proficiency level (e.g. "Beginner", "Intermediate", "Advanced", "Expert").
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// Date the skill was last certified/validated, if applicable.
        /// </summary>
        public DateTime? LastCertifiedDate { get; set; }

        /// <summary>
        /// Name of the certification, if any (e.g. "AWS Solutions Architect").
        /// </summary>
        [MaxLength(300)]
        public string? CertificationName { get; set; }

        /// <summary>
        /// Owner of the skill record — the employee or entity this skill belongs to.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Owner { get; set; } = string.Empty;

        /// <summary>
        /// Source of the skill record (e.g. "DOS", "SelfReport", "ManagerValidation").
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Soft-delete flag. When false the record is considered deleted.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// UTC timestamp when the record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UTC timestamp when the record was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
