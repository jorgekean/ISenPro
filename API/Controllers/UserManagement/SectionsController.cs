using API.Dto;
using EF.Models.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.UserManagement.Interface;

namespace API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly ISectionService _sectionService;
        private readonly ILogger<SectionsController> _logger;

        public SectionsController(ISectionService sectionService, ILogger<SectionsController> logger)
        {
            _sectionService = sectionService;
            _logger = logger;
        }

        // GET: api/Sections
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<SectionDto>>> GetSections([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _sectionService.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<SectionDto>
                {
                    Items = paginatedResult.Data,
                    TotalCount = paginatedResult.TotalRecords,
                    PageNumber = pagingParameters.PageNumber,
                    PageSize = pagingParameters.PageSize
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }


        // GET: api/Sections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SectionDto>> GetSection(int id)
        {
            var umSection = await _sectionService.GetByIdAsync(id);

            if (umSection == null)
            {
                return NotFound();
            }

            return umSection;
        }

        // PUT: api/Sections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSection(string id, SectionDto umSection)
        {
            if (id != umSection.Code)
            {
                return BadRequest();
            }

            try
            {
                await _sectionService.UpdateAsync(umSection);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await sectionExists(id)))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        // POST: api/Sections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SectionDto>> PostUmSection(SectionDto umSection)
        {
            //_context.UmSections.Add(umSection);
            try
            {
                await _sectionService.AddAsync(umSection);
            }
            catch (DbUpdateException ex)
            {
                if (await sectionExists(umSection.Code))
                {
                    return Conflict();
                }
                else
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return CreatedAtAction("GetSection", new { id = umSection.Code }, umSection);
        }

        //// DELETE: api/Sections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmSection(int id)
        {
            //    var section = await _sectionService.GetByIdAsync(id);
            //    if (section == null)
            //    {
            //        return NotFound();
            //    }

            try
            {
                await _sectionService.DeleteAsync(id);
            }
            catch (DbUpdateException ex)
            {
                //if (!(await sectionExists(id)))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    _logger.LogError(ex, ex.Message);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        private async Task<bool> sectionExists(string id)
        {
            return (await _sectionService.GetAllAsync()).Any(e => e.Code == id);
        }
    }
}
