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
    public class DivisionsController : ControllerBase
    {
        private readonly IDivisionService _divisionService;
        private readonly ILogger<DivisionsController> _logger;

        public DivisionsController(IDivisionService divisionService, ILogger<DivisionsController> logger)
        {
            _divisionService = divisionService;
            _logger = logger;
        }

        // GET: api/Divisions
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<DivisionDto>>> GetDivisions([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _divisionService.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<DivisionDto>
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


        // GET: api/Divisions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DivisionDto>> GetDivision(int id)
        {
            var umDivision = await _divisionService.GetByIdAsync(id);

            if (umDivision == null)
            {
                return NotFound();
            }

            return umDivision;
        }

        // PUT: api/Divisions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDivision(string id, DivisionDto umDivision)
        {
            if (id != umDivision.Code)
            {
                return BadRequest();
            }

            try
            {
                await _divisionService.UpdateAsync(umDivision);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await divisionExists(id)))
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

        // POST: api/Divisions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DivisionDto>> PostUmDivision(DivisionDto umDivision)
        {
            //_context.UmDivisions.Add(umDivision);
            try
            {
                var entity = (UmDivision)(await _divisionService.AddAsync(umDivision));
                umDivision.Id = Convert.ToInt32(entity.DivisionId);

                return CreatedAtAction("GetDivision", new { id = entity.DivisionId }, new { id = entity.DivisionId });
            }
            catch (DbUpdateException ex)
            {
                if (await divisionExists(umDivision.Code))
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

            return BadRequest();
        }

        //// DELETE: api/Divisions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmDivision(int id)
        {
            //    var division = await _divisionService.GetByIdAsync(id);
            //    if (division == null)
            //    {
            //        return NotFound();
            //    }

            try
            {
                await _divisionService.DeleteAsync(id);
            }
            catch (DbUpdateException ex)
            {
                //if (!(await divisionExists(id)))
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

        private async Task<bool> divisionExists(string id)
        {
            return (await _divisionService.GetAllAsync()).Any(e => e.Code == id);
        }
    }
}
