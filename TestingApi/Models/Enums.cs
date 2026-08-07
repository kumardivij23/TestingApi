namespace TestingApi.Models
{
    /// <summary>
    /// Represents the proficiency level of a skill.
    /// </summary>
    public enum ProficiencyLevel
    {
        Beginner = 0,
        Intermediate = 1,
        Advanced = 2,
        Expert = 3
    }

    /// <summary>
    /// Represents the certification status of a skill.
    /// </summary>
    public enum CertificationStatus
    {
        NotCertified = 0,
        Pending = 1,
        Certified = 2,
        Expired = 3
    }

    /// <summary>
    /// Represents the type of audit action performed on a skill.
    /// </summary>
    public enum AuditAction
    {
        Created = 0,
        Updated = 1,
        Deleted = 2,
        Certified = 3,
        CertificationExpired = 4
    }
}
