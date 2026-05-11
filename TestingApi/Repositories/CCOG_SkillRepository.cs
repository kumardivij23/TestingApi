using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Models;

namespace TestingApi.Repositories
{
    /// <summary>
    /// EF Core implementation of the CCOG_Skill repository.
    /// </summary>
    public class CCOG_SkillRepository : ICCOG_SkillRepository
    {
        private readonly AppDbContext _context;

        public CCOG_SkillRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CCOG_Skill>> GetAllAsync(string? category = null, string? owner = null, string? source = null)
        {
            IQueryable<CCOG_Skill> query = _context.CCOG_Skills.Where(s => s.IsActive);

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(s => s.Category == category);
            }

            if (!string.IsNullOrWhiteSpace(owner))
            {
                query = query.Where(s => s.Owner == owner);
            }

            if (!string.IsNullOrWhiteSpace(source))
            {
                query = query.Where(s => s.Source == source);
            }

            return await query.OrderBy(s => s.SkillName).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<CCOG_Skill?> GetByIdAsync(int id)
        {
            return await _context.CCOG_Skills
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
        }

        /// <inheritdoc />
        public async Task<CCOG_Skill> CreateAsync(CCOG_Skill skill)
        {
            skill.CreatedAt = DateTime.UtcNow;
            skill.UpdatedAt = DateTime.UtcNow;
            skill.IsActive = true;

            _context.CCOG_Skills.Add(skill);
            await _context.SaveChangesAsync();

            return skill;
        }

        /// <inheritdoc />
        public async Task<CCOG_Skill?> UpdateAsync(CCOG_Skill skill)
        {
            var existing = await _context.CCOG_Skills
                .FirstOrDefaultAsync(s => s.Id == skill.Id && s.IsActive);

            if (existing == null)
            {
                return null;
            }

            existing.SkillName = skill.SkillName;
            existing.Category = skill.Category;
            existing.Level = skill.Level;
            existing.LastCertifiedDate = skill.LastCertifiedDate;
            existing.CertificationName = skill.CertificationName;
            existing.Owner = skill.Owner;
            existing.Source = skill.Source;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existing;
        }

        /// <inheritdoc />
        public async Task<bool> SoftDeleteAsync(int id)
        {
            var existing = await _context.CCOG_Skills
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

            if (existing == null)
            {
                return false;
            }

            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CCOG_Skill>> BulkImportAsync(IEnumerable<CCOG_Skill> skills)
        {
            var now = DateTime.UtcNow;
            var skillList = skills.ToList();

            foreach (var skill in skillList)
            {
                skill.CreatedAt = now;
                skill.UpdatedAt = now;
                skill.IsActive = true;
            }

            await _context.CCOG_Skills.AddRangeAsync(skillList);
            await _context.SaveChangesAsync();

            return skillList;
        }
    }
}
