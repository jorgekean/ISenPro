using API.Dto;
using EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Dto.SystemSetup;
using Service.SystemSetup;
using Service.SystemSetup.Interface;

namespace API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplementaryCataloguesController : ControllerBase
    {
        private readonly ISupplementaryCatalogueService _supplementaryCataloguesservice;
        private readonly ILogger<SupplementaryCataloguesController> _logger;

        public SupplementaryCataloguesController(ISupplementaryCatalogueService supplementaryCataloguesservice, ILogger<SupplementaryCataloguesController> logger)
        {
            _supplementaryCataloguesservice = supplementaryCataloguesservice;
            _logger = logger;
        }

        // GET: api/SupplementaryCatalogues/UnitOfMeasurements
        [HttpGet("UnitOfMeasurements")]
        public async Task<ActionResult<PaginatedResponse<UnitOfMeasurementDto>>> GetUnitOfMeasurements()
        {
            try
            {
                var response = await _supplementaryCataloguesservice.GetUnitOfMeasurements();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/PSDBMCatalogues/AccountCodes
        [HttpGet("AccountCodes")]
        public async Task<ActionResult<PaginatedResponse<UnitOfMeasurementDto>>> GetAccountCodes()
        {
            try
            {
                var response = await _supplementaryCataloguesservice.GetAccountCodes();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/SupplementaryCatalogues
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<VSupplementaryCatalogueIndex>>> GetSupplementaryCatalogues([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _supplementaryCataloguesservice.GetComplexPagedAndFilteredAsync<VSupplementaryCatalogueIndex>(pagingParameters);

                var response = new PaginatedResponse<VSupplementaryCatalogueIndex>
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


        // GET: api/SupplementaryCatalogues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplementaryCatalogueDto>> GetSupplementaryCatalogue(int id)
        {
            try
            {
                var umSupplementaryCatalogue = await _supplementaryCataloguesservice.GetByIdAsync(id);

                if (umSupplementaryCatalogue == null)
                {
                    return NotFound();
                }

                return umSupplementaryCatalogue;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
            }


            return BadRequest();
        }

        // PUT: api/SupplementaryCatalogues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplementaryCatalogue(string id, SupplementaryCatalogueDto umSupplementaryCatalogue)
        {
            if (id != umSupplementaryCatalogue.Code)
            {
                return BadRequest();
            }

            try
            {
                await _supplementaryCataloguesservice.UpdateAsync(umSupplementaryCatalogue);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await SupplementaryCatalogueExists(umSupplementaryCatalogue.Code)))
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

        // POST: api/SupplementaryCatalogues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SupplementaryCatalogueDto>> PostUmSupplementaryCatalogue(SupplementaryCatalogueDto umSupplementaryCatalogue)
        {
            //_context.UmSupplementaryCatalogues.Add(umSupplementaryCatalogue);
            try
            {
                await _supplementaryCataloguesservice.AddAsync(umSupplementaryCatalogue);
            }
            catch (DbUpdateException ex)
            {
                if (await SupplementaryCatalogueExists(umSupplementaryCatalogue.Code))
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

            return CreatedAtAction("GetSupplementaryCatalogue", new { id = umSupplementaryCatalogue.Code }, umSupplementaryCatalogue);
        }

        //// DELETE: api/SupplementaryCatalogues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmSupplementaryCatalogue(int id)
        {

            try
            {
                await _supplementaryCataloguesservice.DeleteAsync(id);
            }           
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        private async Task<bool> SupplementaryCatalogueExists(string code)
        {
            return (await _supplementaryCataloguesservice.GetAllAsync()).Any(e => e.Code == code);
        }
    }
}
