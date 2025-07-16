using API.Controllers.UserManagement;
using API.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Dto.SystemSetup;
using Service;
using Service.SystemSetup.Interface;
using Service.Transaction.Interface;
using Service.Dto.Transaction;
using Microsoft.EntityFrameworkCore;
using Service.SystemSetup;
using Service.Cache;
using EF.Models;

namespace API.Controllers.Transaction
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrController : ControllerBase
    {
        private readonly IPrService _prService;
        private readonly CachedItems _cachedItems;

        private readonly ILogger<PrController> _logger;

        public PrController(IPrService prService, ILogger<PrController> logger, CachedItems cachedItems)
        {
            _cachedItems = cachedItems;
            _prService = prService;
            _logger = logger;
        }

        #region CRUD
        // GET: api/prs
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<PRDto>>> GetAll([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                pagingParameters.ApplyFilterCriteria = true;
                pagingParameters.ParentModule = 1;

                var paginatedResult = await _prService.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<PRDto>
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

        // GET: api/prs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PRDto>> GetById(int id)
        {
            try
            {
                var model = await _prService.GetByIdAsync(id);

                if (model == null)
                {
                    return NotFound();
                }

                return model;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
            }

            return BadRequest();
        }

        // POST: api/prs        
        [HttpPost]
        public async Task<ActionResult<PRDto>> Post(PRDto model)
        {
            try
            {
                await _prService.AddAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }

            return CreatedAtAction("GetById", new { id = model.Id }, model);
        }

        // PUT: api/prs/5        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PRDto model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            try
            {
                await _prService.UpdateAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        //// DELETE: api/prs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _prService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        #endregion

        // pritems?purchaseRequestId=5
        [HttpGet("pritems")]
        public async Task<ActionResult<IEnumerable<PurchaseRequestItemDetail>>> GetPrItems([FromQuery] int purchaseRequestId)
        {
            try
            {                            
                var result = await _prService.GetPurchaseRequestItems(purchaseRequestId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        // remainingppmpcatalogueitems?budgetYear=2025&requestingOffice=1
        [HttpGet("remainingppmpcatalogueitems")]
        public async Task<ActionResult<IEnumerable<VPpmpPsdbmcatalogue>>> GetPrItems([FromQuery] short budgetYear, [FromQuery] int requestingOffice)
        {
            try
            {
                var result = await _prService.GetRemainingPpmpCatalogue(budgetYear, requestingOffice);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        #region Reports
        //[HttpGet("printpreview")]
        //public async Task<ActionResult<PRDto>> GetPPMPReport(int id)
        //{
        //    try
        //    {
        //        await _prService.GenerateReport(id);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //    }

        //    return BadRequest();
        //}
        #endregion

    }
}
