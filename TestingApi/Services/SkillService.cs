using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.DTOs;
using TestingApi.Models;

namespace TestingApi.Services
{
    /// <summary>
    /// Service implementation for skill management with full CRUD,
    /// certification support, and audit logging.
    /// </summary>
    public class SkillService : ISkillService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<SkillService> _logger;

        public SkillService(AppDbContext dbContext, ILogger<SkillService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<PaginatedResponse<SkillResponse>> GetSkillsByPersonIdAsync(Guid personId, int page, int pageSize)
        {
            var query = _dbContext.Skills.Where(s => s.PersonId == personId);

            var totalCount = await query.CountAsync();

            var skills = await query
                .OrderByDescending(s => s.UpdatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<SkillResponse>
            {
                Items = skills.Select(MapToResponse).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        /// <inheritdoc />
        public async Task<SkillResponse> AddSkillAsync(CreateSkillRequest request)
        {
            var skill = new Skill
            {
                Id = Guid.NewGuid(),
                PersonId = request.PersonId,
                Name = request.Name,
                Description = request.Description,
                ProficiencyLevel = request.ProficiencyLevel,
                CertificationStatus = CertificationStatus.NotCertified,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            _dbContext.Skills.Add(skill);

            await LogAuditAsync(skill.Id, skill.PersonId, AuditAction.Created,
                $"Skill '{skill.Name}' created with proficiency level '{skill.ProficiencyLevel}'.",
                previousValues: null,
                newValues: SerializeSkill(skill));

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Skill '{SkillName}' (ID: {SkillId}) created for person {PersonId}.",
                skill.Name, skill.Id, skill.PersonId);

            return MapToResponse(skill);
        }

        /// <inheritdoc />
        public async Task<SkillResponse?> UpdateSkillAsync(Guid skillId, UpdateSkillRequest request)
        {
            var skill = await _dbContext.Skills.FindAsync(skillId);
            if (skill == null)
            {
                _logger.LogWarning("Skill with ID {SkillId} not found for update.", skillId);
                return null;
            }

            var previousValues = SerializeSkill(skill);

            if (request.Name != null)
                skill.Name = request.Name;

            if (request.Description != null)
                skill.Description = request.Description;

            if (request.ProficiencyLevel.HasValue)
                skill.ProficiencyLevel = request.ProficiencyLevel.Value;

            skill.UpdatedAt = DateTime.UtcNow;

            var newValues = SerializeSkill(skill);

            await LogAuditAsync(skill.Id, skill.PersonId, AuditAction.Updated,
                $"Skill '{skill.Name}' updated.",
                previousValues: previousValues,
                newValues: newValues);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Skill '{SkillName}' (ID: {SkillId}) updated.", skill.Name, skill.Id);

            return MapToResponse(skill);
        }

        /// <inheritdoc />
        public async Task<bool> RemoveSkillAsync(Guid skillId)
        {
            var skill = await _dbContext.Skills.FindAsync(skillId);
            if (skill == null)
            {
                _logger.LogWarning("Skill with ID {SkillId} not found for deletion.", skillId);
                return false;
            }

            var previousValues = SerializeSkill(skill);

            skill.IsDeleted = true;
            skill.UpdatedAt = DateTime.UtcNow;

            await LogAuditAsync(skill.Id, skill.PersonId, AuditAction.Deleted,
                $"Skill '{skill.Name}' soft-deleted.",
                previousValues: previousValues,
                newValues: null);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Skill '{SkillName}' (ID: {SkillId}) soft-deleted.", skill.Name, skill.Id);

            return true;
        }

        /// <inheritdoc />
        public async Task<SkillResponse?> CertifySkillAsync(Guid skillId, CertifySkillRequest request)
        {
            var skill = await _dbContext.Skills.FindAsync(skillId);
            if (skill == null)
            {
                _logger.LogWarning("Skill with ID {SkillId} not found for certification.", skillId);
                return null;
            }

            var previousValues = SerializeSkill(skill);

            skill.CertificationStatus = request.CertificationStatus;
            skill.CertifiedDate = request.CertifiedDate ?? DateTime.UtcNow;
            skill.CertificationExpiryDate = request.CertificationExpiryDate;
            skill.CertifyingAuthority = request.CertifyingAuthority;
            skill.UpdatedAt = DateTime.UtcNow;

            var newValues = SerializeSkill(skill);

            await LogAuditAsync(skill.Id, skill.PersonId, AuditAction.Certified,
                $"Skill '{skill.Name}' certification updated to '{request.CertificationStatus}'" +
                (request.CertifyingAuthority != null ? $" by '{request.CertifyingAuthority}'." : "."),
                previousValues: previousValues,
                newValues: newValues);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Skill '{SkillName}' (ID: {SkillId}) certified with status '{Status}'.",
                skill.Name, skill.Id, request.CertificationStatus);

            return MapToResponse(skill);
        }

        /// <inheritdoc />
        public async Task<PaginatedResponse<SkillAuditLogResponse>> GetAuditLogsBySkillIdAsync(Guid skillId, int page, int pageSize)
        {
            var query = _dbContext.SkillAuditLogs.Where(a => a.SkillId == skillId);

            var totalCount = await query.CountAsync();

            var logs = await query
                .OrderByDescending(a => a.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<SkillAuditLogResponse>
            {
                Items = logs.Select(MapToAuditResponse).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        /// <inheritdoc />
        public async Task<PaginatedResponse<SkillAuditLogResponse>> GetAuditLogsByPersonIdAsync(Guid personId, int page, int pageSize)
        {
            var query = _dbContext.SkillAuditLogs.Where(a => a.PersonId == personId);

            var totalCount = await query.CountAsync();

            var logs = await query
                .OrderByDescending(a => a.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<SkillAuditLogResponse>
            {
                Items = logs.Select(MapToAuditResponse).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        // ──────────────────────────────────────────────
        //  Private Helpers
        // ──────────────────────────────────────────────

        private async Task LogAuditAsync(Guid skillId, Guid personId, AuditAction action,
            string details, string? previousValues, string? newValues)
        {
            var auditLog = new SkillAuditLog
            {
                Id = Guid.NewGuid(),
                SkillId = skillId,
                PersonId = personId,
                Action = action,
                Details = details,
                PreviousValues = previousValues,
                NewValues = newValues,
                Timestamp = DateTime.UtcNow
            };

            _dbContext.SkillAuditLogs.Add(auditLog);
        }

        private static string SerializeSkill(Skill skill)
        {
            return JsonSerializer.Serialize(new
            {
                skill.Id,
                skill.PersonId,
                skill.Name,
                skill.Description,
                ProficiencyLevel = skill.ProficiencyLevel.ToString(),
                CertificationStatus = skill.CertificationStatus.ToString(),
                skill.CertifiedDate,
                skill.CertificationExpiryDate,
                skill.CertifyingAuthority,
                skill.CreatedAt,
                skill.UpdatedAt
            });
        }

        private static SkillResponse MapToResponse(Skill skill)
        {
            return new SkillResponse
            {
                Id = skill.Id,
                PersonId = skill.PersonId,
                Name = skill.Name,
                Description = skill.Description,
                ProficiencyLevel = skill.ProficiencyLevel,
                CertificationStatus = skill.CertificationStatus,
                CertifiedDate = skill.CertifiedDate,
                CertificationExpiryDate = skill.CertificationExpiryDate,
                CertifyingAuthority = skill.CertifyingAuthority,
                CreatedAt = skill.CreatedAt,
                UpdatedAt = skill.UpdatedAt
            };
        }

        private static SkillAuditLogResponse MapToAuditResponse(SkillAuditLog log)
        {
            return new SkillAuditLogResponse
            {
                Id = log.Id,
                SkillId = log.SkillId,
                PersonId = log.PersonId,
                Action = log.Action,
                ActionName = log.Action.ToString(),
                Details = log.Details,
                PreviousValues = log.PreviousValues,
                NewValues = log.NewValues,
                Timestamp = log.Timestamp
            };
        }
    }
}
