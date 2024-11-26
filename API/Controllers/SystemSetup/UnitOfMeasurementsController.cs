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
    public class UnitOfMeasurementsController : ControllerBase
    {
        private readonly IUnitOfMeasurementService _uomService;
        private readonly ILogger<UnitOfMeasurementsController> _logger;

        public UnitOfMeasurementsController(IUnitOfMeasurementService unitofmeasurementService, ILogger<UnitOfMeasurementsController> logger)
        {
            _uomService = unitofmeasurementService;
            _logger = logger;
        }

        // GET: api/UnitOfMeasurements
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<UnitOfMeasurementDto>>> GetUOMs([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _uomService.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<UnitOfMeasurementDto>
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


        // GET: api/UnitOfMeasurements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UnitOfMeasurementDto>> GetUnitOfMeasurement(int id)
        {
            try
            {
                var umUnitOfMeasurement = await _uomService.GetByIdAsync(id);

                if (umUnitOfMeasurement == null)
                {
                    return NotFound();
                }

                return umUnitOfMeasurement;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
            }


            return BadRequest();
        }

        // PUT: api/UnitOfMeasurements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnitOfMeasurement(string id, UnitOfMeasurementDto umUnitOfMeasurement)
        {
            if (id != umUnitOfMeasurement.Code)
            {
                return BadRequest();
            }

            try
            {
                await _uomService.UpdateAsync(umUnitOfMeasurement);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await unitofmeasurementExists(umUnitOfMeasurement.Code)))
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

        // POST: api/UnitOfMeasurements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UnitOfMeasurementDto>> PostUmUnitOfMeasurement(UnitOfMeasurementDto umUnitOfMeasurement)
        {
            //_context.UmUnitOfMeasurements.Add(umUnitOfMeasurement);
            try
            {
                await _uomService.AddAsync(umUnitOfMeasurement);
            }
            catch (DbUpdateException ex)
            {
                if (await unitofmeasurementExists(umUnitOfMeasurement.Code))
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

            return CreatedAtAction("GetUnitOfMeasurement", new { id = umUnitOfMeasurement.Code }, umUnitOfMeasurement);
        }

        //// DELETE: api/UnitOfMeasurements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmUnitOfMeasurement(int id)
        {

            try
            {
                await _uomService.DeleteAsync(id);
            }           
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        private async Task<bool> unitofmeasurementExists(string code)
        {
            return (await _uomService.GetAllAsync()).Any(e => e.Code == code);
        }
    }
}
