using API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Dto.SystemSetup;
using Service.Dto.UserManagement;
using Service.Enums;
using Service.Helpers;
using Service.SystemSetup.Interface;

namespace API.Controllers.SystemSetup
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceTablesController : ControllerBase
    {
        private readonly IReferenceTableService _referenceTableService;
        private readonly ILogger<ReferenceTablesController> _logger;

        public ReferenceTablesController(IReferenceTableService referenceTableService, ILogger<ReferenceTablesController> logger)
        {
            _referenceTableService = referenceTableService;
            _logger = logger;
        }

        // GET: api/ReferenceTables
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<ReferenceTableDto>>> GetUOMs([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _referenceTableService.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<ReferenceTableDto>
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

        // GET: api/referencetables/referencetableparents
        [HttpGet("referencetableparents")]
        public ActionResult<IEnumerable<ControlDto>> GetReferenceTableParents()
        {
            try
            {
                var enumList = EnumHelper.GetEnumList<ReferenceTableModule>();

                // Transform the enum list to match the desired response structure
                var list = enumList.Select(e => new
                {
                    Id = e.Value,
                    Label = e.Description
                }).ToList();

                var response = new PaginatedResponse<object>
                {
                    Items = list
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/ReferenceTables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReferenceTableDto>> GetReferenceTable(int id)
        {
            try
            {
                var umReferenceTable = await _referenceTableService.GetByIdAsync(id);

                if (umReferenceTable == null)
                {
                    return NotFound();
                }

                return umReferenceTable;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
            }


            return BadRequest();
        }

        // PUT: api/ReferenceTables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReferenceTable(string id, ReferenceTableDto umReferenceTable)
        {
            if (id != umReferenceTable.Id.ToString())
            {
                return BadRequest();
            }

            try
            {
                await _referenceTableService.UpdateAsync(umReferenceTable);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await referenceTableDataExists(umReferenceTable.Code)))
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

        // POST: api/ReferenceTables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReferenceTableDto>> PostUmReferenceTable(ReferenceTableDto umReferenceTable)
        {
            //_context.UmReferenceTables.Add(umReferenceTable);
            try
            {
                await _referenceTableService.AddAsync(umReferenceTable);
            }
            catch (DbUpdateException ex)
            {
                if (await referenceTableDataExists(umReferenceTable.Code))
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

            return CreatedAtAction("GetReferenceTable", new { id = umReferenceTable.Code }, umReferenceTable);
        }

        //// DELETE: api/ReferenceTables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmReferenceTable(int id)
        {

            try
            {
                await _referenceTableService.DeleteAsync(id);
            }           
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        private async Task<bool> referenceTableDataExists(string code)
        {
            return (await _referenceTableService.GetAllAsync()).Any(e => e.Code == code);
        }
    }
}
