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
    public class PurchasingTypesController : ControllerBase
    {
        private readonly IPurchasingTypeService _purchasingTypeService;
        private readonly ILogger<PurchasingTypesController> _logger;

        public PurchasingTypesController(IPurchasingTypeService purchasingtypeService, ILogger<PurchasingTypesController> logger)
        {
            _purchasingTypeService = purchasingtypeService;
            _logger = logger;
        }

        // GET: api/PurchasingTypes
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<PurchasingTypeDto>>> GetUOMs([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _purchasingTypeService.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<PurchasingTypeDto>
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


        // GET: api/PurchasingTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchasingTypeDto>> GetPurchasingType(int id)
        {
            try
            {
                var umPurchasingType = await _purchasingTypeService.GetByIdAsync(id);

                if (umPurchasingType == null)
                {
                    return NotFound();
                }

                return umPurchasingType;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
            }


            return BadRequest();
        }

        // PUT: api/PurchasingTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchasingType(string id, PurchasingTypeDto umPurchasingType)
        {
            if (id != umPurchasingType.Code)
            {
                return BadRequest();
            }

            try
            {
                await _purchasingTypeService.UpdateAsync(umPurchasingType);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await purchasingtypeExists(umPurchasingType.Code)))
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

        // POST: api/PurchasingTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PurchasingTypeDto>> PostUmPurchasingType(PurchasingTypeDto umPurchasingType)
        {
            //_context.UmPurchasingTypes.Add(umPurchasingType);
            try
            {
                await _purchasingTypeService.AddAsync(umPurchasingType);
            }
            catch (DbUpdateException ex)
            {
                if (await purchasingtypeExists(umPurchasingType.Code))
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

            return CreatedAtAction("GetPurchasingType", new { id = umPurchasingType.Code }, umPurchasingType);
        }

        //// DELETE: api/PurchasingTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmPurchasingType(int id)
        {

            try
            {
                await _purchasingTypeService.DeleteAsync(id);
            }           
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        private async Task<bool> purchasingtypeExists(string code)
        {
            return (await _purchasingTypeService.GetAllAsync()).Any(e => e.Code == code);
        }
    }
}
