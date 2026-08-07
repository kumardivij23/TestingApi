using System.ComponentModel.DataAnnotations;

namespace TestingApi.Models
{
    /// <summary>
    /// Represents a skill associated with a person in the CCOG system.
    /// </summary>
    public class Skill
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The identifier of the person who owns this skill.
        /// </summary>
        [Required]
        public Guid PersonId { get; set; }

        /// <summary>
        /// The name of the skill (e.g., "C#", "Azure DevOps").
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Optional description providing more details about the skill.
        /// </summary>
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// The proficiency level of the person in this skill.
        /// </summary>
        public ProficiencyLevel ProficiencyLevel { get; set; } = ProficiencyLevel.Beginner;

        /// <summary>
        /// The current certification status of this skill.
        /// </summary>
        public CertificationStatus CertificationStatus { get; set; } = CertificationStatus.NotCertified;

        /// <summary>
        /// The date when the skill was certified (if applicable).
        /// </summary>
        public DateTime? CertifiedDate { get; set; }

        /// <summary>
        /// The date when the certification expires (if applicable).
        /// </summary>
        public DateTime? CertificationExpiryDate { get; set; }

        /// <summary>
        /// The name or identifier of the certifying authority.
        /// </summary>
        [MaxLength(300)]
        public string? CertifyingAuthority { get; set; }

        /// <summary>
        /// The date and time when the skill record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The date and time when the skill record was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates whether the skill record has been soft-deleted.
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
