using Microsoft.AspNetCore.Mvc;
using TestingApi.Models;
using TestingApi.Repositories;

namespace TestingApi.Controllers
{
    /// <summary>
    /// API controller for managing unified CCOG Skill records.
    /// Supports list/filter, get-by-id, create, update, soft-delete, and bulk import.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CCOG_SkillsController : ControllerBase
    {
        private readonly ICCOG_SkillRepository _repository;
        private readonly ILogger<CCOG_SkillsController> _logger;

        public CCOG_SkillsController(ICCOG_SkillRepository repository, ILogger<CCOG_SkillsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// GET api/ccog_skills
        /// Returns all active skills. Supports optional query-string filters: category, owner, source.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CCOG_Skill>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? category = null,
            [FromQuery] string? owner = null,
            [FromQuery] string? source = null)
        {
            _logger.LogInformation("Fetching CCOG Skills — category={Category}, owner={Owner}, source={Source}", category, owner, source);
            var skills = await _repository.GetAllAsync(category, owner, source);
            return Ok(skills);
        }

        /// <summary>
        /// GET api/ccog_skills/{id}
        /// Returns a single active skill by its primary key.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CCOG_Skill), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching CCOG Skill by Id={Id}", id);
            var skill = await _repository.GetByIdAsync(id);

            if (skill == null)
            {
                return NotFound(new { message = $"Skill with Id {id} not found or has been deleted." });
            }

            return Ok(skill);
        }

        /// <summary>
        /// POST api/ccog_skills
        /// Creates a new CCOG Skill record.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CCOG_Skill), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CCOG_Skill skill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating CCOG Skill: {SkillName} for owner {Owner}", skill.SkillName, skill.Owner);
            var created = await _repository.CreateAsync(skill);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// PUT api/ccog_skills/{id}
        /// Updates an existing CCOG Skill record.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(CCOG_Skill), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] CCOG_Skill skill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != skill.Id)
            {
                return BadRequest(new { message = "Route id does not match body id." });
            }

            _logger.LogInformation("Updating CCOG Skill Id={Id}", id);
            var updated = await _repository.UpdateAsync(skill);

            if (updated == null)
            {
                return NotFound(new { message = $"Skill with Id {id} not found or has been deleted." });
            }

            return Ok(updated);
        }

        /// <summary>
        /// DELETE api/ccog_skills/{id}
        /// Soft-deletes a CCOG Skill record (sets IsActive = false).
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Soft-deleting CCOG Skill Id={Id}", id);
            var result = await _repository.SoftDeleteAsync(id);

            if (!result)
            {
                return NotFound(new { message = $"Skill with Id {id} not found or has been deleted." });
            }

            return NoContent();
        }

        /// <summary>
        /// POST api/ccog_skills/bulk
        /// Bulk import multiple CCOG Skill records.
        /// </summary>
        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<CCOG_Skill>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkImport([FromBody] IEnumerable<CCOG_Skill> skills)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Bulk importing {Count} CCOG Skills", skills.Count());
            var imported = await _repository.BulkImportAsync(skills);

            return CreatedAtAction(nameof(GetAll), imported);
        }
    }
}
