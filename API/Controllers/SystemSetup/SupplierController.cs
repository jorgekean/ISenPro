using API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Dto.SystemSetup;
using Service.SystemSetup;
using Service.SystemSetup.Interface;

namespace API.Controllers.SystemSetup
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly ILogger<SuppliersController> _logger;

        public SuppliersController(ISupplierService SupplierService, ILogger<SuppliersController> logger)
        {
            _supplierService = SupplierService;
            _logger = logger;
        }

        // GET: api/Supplier/Industries
        [HttpGet("Industries")]
        public async Task<ActionResult<PaginatedResponse<UnitOfMeasurementDto>>> GetIndustries()
        {
            try
            {
                var response = await _supplierService.GetIndustries();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<SupplierDto>>> GetSuppliers([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _supplierService.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<SupplierDto>
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


        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDto>> GetSupplier(int id)
        {
            try
            {
                var umSupplier = await _supplierService.GetByIdAsync(id);

                if (umSupplier == null)
                {
                    return NotFound();
                }

                return umSupplier;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
            }


            return BadRequest();
        }

        // PUT: api/Suppliers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(string id, SupplierDto umSupplier)
        {
            if (id != umSupplier.CompanyName)
            {
                return BadRequest();
            }

            try
            {
                await _supplierService.UpdateAsync(umSupplier);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await SupplierExists(umSupplier.CompanyName)))
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

        // POST: api/Suppliers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SupplierDto>> PostUmSupplier(SupplierDto umSupplier)
        {
            //_context.UmSuppliers.Add(umSupplier);
            try
            {
                await _supplierService.AddAsync(umSupplier);
            }
            catch (DbUpdateException ex)
            {
                if (await SupplierExists(umSupplier.CompanyName))
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

            return CreatedAtAction("GetSupplier", new { id = umSupplier.CompanyName }, umSupplier);
        }

        //// DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmSupplier(int id)
        {

            try
            {
                await _supplierService.DeleteAsync(id);
            }           
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        private async Task<bool> SupplierExists(string companyName)
        {
            return (await _supplierService.GetAllAsync()).Any(e => e.CompanyName == companyName);
        }
    }
}
