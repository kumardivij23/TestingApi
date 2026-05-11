using TestingApi.DTOs;

namespace TestingApi.Services
{
    /// <summary>
    /// Service interface for skill management operations.
    /// </summary>
    public interface ISkillService
    {
        /// <summary>
        /// Gets paginated skills for a specific person.
        /// </summary>
        Task<PaginatedResponse<SkillResponse>> GetSkillsByPersonIdAsync(Guid personId, int page, int pageSize);

        /// <summary>
        /// Adds a new skill for a person.
        /// </summary>
        Task<SkillResponse> AddSkillAsync(CreateSkillRequest request);

        /// <summary>
        /// Updates an existing skill.
        /// </summary>
        Task<SkillResponse?> UpdateSkillAsync(Guid skillId, UpdateSkillRequest request);

        /// <summary>
        /// Soft-deletes a skill by ID.
        /// </summary>
        Task<bool> RemoveSkillAsync(Guid skillId);

        /// <summary>
        /// Certifies or updates certification status for a skill.
        /// </summary>
        Task<SkillResponse?> CertifySkillAsync(Guid skillId, CertifySkillRequest request);

        /// <summary>
        /// Gets paginated audit logs for a specific skill.
        /// </summary>
        Task<PaginatedResponse<SkillAuditLogResponse>> GetAuditLogsBySkillIdAsync(Guid skillId, int page, int pageSize);

        /// <summary>
        /// Gets paginated audit logs for a specific person.
        /// </summary>
        Task<PaginatedResponse<SkillAuditLogResponse>> GetAuditLogsByPersonIdAsync(Guid personId, int page, int pageSize);
    }
}
