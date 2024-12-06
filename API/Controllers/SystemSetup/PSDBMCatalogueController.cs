using API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Dto.SystemSetup;
using Service.SystemSetup.Interface;

namespace API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class PSDBMCataloguesController : ControllerBase
    {
        private readonly IPSDBMCatalogueService _psdbmCataloguesservice;
        private readonly ILogger<PSDBMCataloguesController> _logger;

        public PSDBMCataloguesController(IPSDBMCatalogueService psdbmCataloguesservice, ILogger<PSDBMCataloguesController> logger)
        {
            _psdbmCataloguesservice = psdbmCataloguesservice;
            _logger = logger;
        }

        // GET: api/PSDBMCatalogues/UnitOfMeasurements
        [HttpGet("UnitOfMeasurements")]
        public async Task<ActionResult<PaginatedResponse<UnitOfMeasurementDto>>> GetUnitOfMeasurements()
        {
            try
            {
                var response = await _psdbmCataloguesservice.GetUnitOfMeasurements();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/PSDBMCatalogues
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<PSDBMCatalogueDto>>> GetPSDBMCatalogues([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _psdbmCataloguesservice.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<PSDBMCatalogueDto>
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


        // GET: api/PSDBMCatalogues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PSDBMCatalogueDto>> GetPSDBMCatalogue(int id)
        {
            try
            {
                var umPSDBMCatalogue = await _psdbmCataloguesservice.GetByIdAsync(id);

                if (umPSDBMCatalogue == null)
                {
                    return NotFound();
                }

                return umPSDBMCatalogue;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
            }


            return BadRequest();
        }

        // PUT: api/PSDBMCatalogues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPSDBMCatalogue(string id, PSDBMCatalogueDto umPSDBMCatalogue)
        {
            if (id != umPSDBMCatalogue.Code)
            {
                return BadRequest();
            }

            try
            {
                await _psdbmCataloguesservice.UpdateAsync(umPSDBMCatalogue);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await psdbmCatalogueExists(umPSDBMCatalogue.Code)))
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

        // POST: api/PSDBMCatalogues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PSDBMCatalogueDto>> PostUmPSDBMCatalogue(PSDBMCatalogueDto umPSDBMCatalogue)
        {
            //_context.UmPSDBMCatalogues.Add(umPSDBMCatalogue);
            try
            {
                await _psdbmCataloguesservice.AddAsync(umPSDBMCatalogue);
            }
            catch (DbUpdateException ex)
            {
                if (await psdbmCatalogueExists(umPSDBMCatalogue.Code))
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

            return CreatedAtAction("GetPSDBMCatalogue", new { id = umPSDBMCatalogue.Code }, umPSDBMCatalogue);
        }

        //// DELETE: api/PSDBMCatalogues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmPSDBMCatalogue(int id)
        {

            try
            {
                await _psdbmCataloguesservice.DeleteAsync(id);
            }           
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        private async Task<bool> psdbmCatalogueExists(string code)
        {
            return (await _psdbmCataloguesservice.GetAllAsync()).Any(e => e.Code == code);
        }
    }
}
