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
    public class AppController : ControllerBase
    {
        private readonly IAppService _appService;
        private readonly CachedItems _cachedItems;

        private readonly ILogger<AppController> _logger;

        public AppController(IAppService appService, ILogger<AppController> logger, CachedItems cachedItems)
        {
            _cachedItems = cachedItems;
            _appService = appService;
            _logger = logger;
        }

        #region CRUD
        // GET: api/ppmps
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<VPpmpindex>>> GetAll([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                pagingParameters.ApplyFilterCriteria = true;
                pagingParameters.ParentModule = 1;

                var paginatedResult = await _appService.GetComplexPagedAndFilteredAsync<VPpmpindex>(pagingParameters);

                var response = new PaginatedResponse<VPpmpindex>
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

        // GET: api/ppmps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<APPDto>> GetById(int id)
        {
            try
            {
                var model = await _appService.GetByIdAsync(id);

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

        // POST: api/ppmps        
        [HttpPost]
        public async Task<ActionResult<APPDto>> Post(APPDto model)
        {
            try
            {
                await _appService.AddAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }

            return CreatedAtAction("GetById", new { id = model.Id }, model);
        }

        // PUT: api/ppmps/5        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, APPDto model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            try
            {
                await _appService.UpdateAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        //// DELETE: api/ppmps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _appService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        #endregion

        // OfficeNoSubmittedPPMP
        [HttpGet("officesnosubmittedppmp")]
        public async Task<ActionResult<IEnumerable<APPDetailsPPMPDto>>> GetOfficeNoSubmittedPPMP([FromQuery] short budgetYear)
        {
            try
            {                            
                var result = await _appService.GetOfficesNoPPMPs(budgetYear);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        // OfficesWithApprovedPPMP
        [HttpGet("officeswithapprovedppmp")]
        public async Task<ActionResult<IEnumerable<APPDetailsPPMPDto>>> GetOfficesWithApprovedPPMP([FromQuery] short budgetYear)
        {
            try
            {
                var result = await _appService.GetOfficesWithApprovedPPMPs(budgetYear);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        // OfficesWithSavedPPMP
        [HttpGet("officeswithsavedppmp")]
        public async Task<ActionResult<IEnumerable<APPDetailsPPMPDto>>> GetOfficesWithSavedPPMP([FromQuery] short budgetYear)
        {
            try
            {
                var result = await _appService.GetOfficesWithSavedPPMPs(budgetYear);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        // viewconsolidated
        [HttpGet("viewconsolidated")]
        public async Task<ActionResult<IEnumerable<APPCatalogueDto>>> GetConsolidated([FromQuery] short budgetYear)
        {
            try
            {
                var result = await _appService.ViewConsolidated(budgetYear);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        #region Reports
        [HttpGet("printpreview")]
        public async Task<ActionResult<APPDto>> GetPPMPReport(int id)
        {
            try
            {
                await _appService.GenerateReport(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest();
        }
        #endregion

    }
}
