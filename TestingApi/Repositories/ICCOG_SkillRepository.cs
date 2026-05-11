using TestingApi.Models;

namespace TestingApi.Repositories
{
    /// <summary>
    /// Repository contract for CCOG_Skill CRUD and bulk operations.
    /// </summary>
    public interface ICCOG_SkillRepository
    {
        /// <summary>
        /// Get all active skills, optionally filtered by category, owner, or source.
        /// </summary>
        Task<IEnumerable<CCOG_Skill>> GetAllAsync(string? category = null, string? owner = null, string? source = null);

        /// <summary>
        /// Get a single skill by its primary key (only if active).
        /// </summary>
        Task<CCOG_Skill?> GetByIdAsync(int id);

        /// <summary>
        /// Create a new skill record.
        /// </summary>
        Task<CCOG_Skill> CreateAsync(CCOG_Skill skill);

        /// <summary>
        /// Update an existing skill record.
        /// </summary>
        Task<CCOG_Skill?> UpdateAsync(CCOG_Skill skill);

        /// <summary>
        /// Soft-delete a skill record (sets IsActive = false).
        /// </summary>
        Task<bool> SoftDeleteAsync(int id);

        /// <summary>
        /// Bulk import a collection of skill records.
        /// </summary>
        Task<IEnumerable<CCOG_Skill>> BulkImportAsync(IEnumerable<CCOG_Skill> skills);
    }
}
