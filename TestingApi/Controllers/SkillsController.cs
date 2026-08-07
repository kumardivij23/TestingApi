using Microsoft.AspNetCore.Mvc;
using TestingApi.DTOs;
using TestingApi.Services;

namespace TestingApi.Controllers
{
    /// <summary>
    /// API controller for managing skills in the CCOG system.
    /// Provides endpoints for CRUD operations, certification, and audit log retrieval.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;
        private readonly ILogger<SkillsController> _logger;

        public SkillsController(ISkillService skillService, ILogger<SkillsController> logger)
        {
            _skillService = skillService;
            _logger = logger;
        }

        /// <summary>
        /// GET api/skills/person/{personId}
        /// Retrieves paginated skills for a specific person.
        /// </summary>
        [HttpGet("person/{personId:guid}")]
        [ProducesResponseType(typeof(PaginatedResponse<SkillResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSkillsByPerson(Guid personId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var result = await _skillService.GetSkillsByPersonIdAsync(personId, page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// POST api/skills
        /// Adds a new skill for a person.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SkillResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddSkill([FromBody] CreateSkillRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _skillService.AddSkillAsync(request);
            return CreatedAtAction(nameof(GetSkillsByPerson), new { personId = result.PersonId }, result);
        }

        /// <summary>
        /// PUT api/skills/{skillId}
        /// Updates an existing skill.
        /// </summary>
        [HttpPut("{skillId:guid}")]
        [ProducesResponseType(typeof(SkillResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSkill(Guid skillId, [FromBody] UpdateSkillRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _skillService.UpdateSkillAsync(skillId, request);
            if (result == null)
                return NotFound(new { message = $"Skill with ID '{skillId}' not found." });

            return Ok(result);
        }

        /// <summary>
        /// DELETE api/skills/{skillId}
        /// Soft-deletes a skill by ID.
        /// </summary>
        [HttpDelete("{skillId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveSkill(Guid skillId)
        {
            var success = await _skillService.RemoveSkillAsync(skillId);
            if (!success)
                return NotFound(new { message = $"Skill with ID '{skillId}' not found." });

            return NoContent();
        }

        /// <summary>
        /// PATCH api/skills/{skillId}/certify
        /// Certifies or updates the certification status of a skill.
        /// </summary>
        [HttpPatch("{skillId:guid}/certify")]
        [ProducesResponseType(typeof(SkillResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CertifySkill(Guid skillId, [FromBody] CertifySkillRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _skillService.CertifySkillAsync(skillId, request);
            if (result == null)
                return NotFound(new { message = $"Skill with ID '{skillId}' not found." });

            return Ok(result);
        }

        /// <summary>
        /// GET api/skills/{skillId}/audit-logs
        /// Retrieves paginated audit logs for a specific skill.
        /// </summary>
        [HttpGet("{skillId:guid}/audit-logs")]
        [ProducesResponseType(typeof(PaginatedResponse<SkillAuditLogResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuditLogsBySkill(Guid skillId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var result = await _skillService.GetAuditLogsBySkillIdAsync(skillId, page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// GET api/skills/person/{personId}/audit-logs
        /// Retrieves paginated audit logs for all skills of a specific person.
        /// </summary>
        [HttpGet("person/{personId:guid}/audit-logs")]
        [ProducesResponseType(typeof(PaginatedResponse<SkillAuditLogResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuditLogsByPerson(Guid personId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var result = await _skillService.GetAuditLogsByPersonIdAsync(personId, page, pageSize);
            return Ok(result);
        }
    }
}
