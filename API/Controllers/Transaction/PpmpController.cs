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

namespace API.Controllers.Transaction
{
    [Route("api/[controller]")]
    [ApiController]
    public class PpmpController : ControllerBase
    {
        private readonly IPpmpService _ppmpService;
        private readonly CachedItems _cachedItems;

        private readonly ILogger<PpmpController> _logger;

        public PpmpController(IPpmpService ppmpService, ILogger<PpmpController> logger, CachedItems cachedItems)
        {
            _cachedItems = cachedItems;
            _ppmpService = ppmpService;
            _logger = logger;
        }

        #region CRUD
        // GET: api/ppmps
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<PPMPDto>>> GetAll([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _ppmpService.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<PPMPDto>
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
        public async Task<ActionResult<PPMPDto>> GetById(int id)
        {
            try
            {
                var model = await _ppmpService.GetByIdAsync(id);

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
        public async Task<ActionResult<PPMPDto>> Post(PPMPDto model)
        {
            try
            {
                await _ppmpService.AddAsync(model);
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
        public async Task<IActionResult> Put(int id, PPMPDto model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            try
            {
                await _ppmpService.UpdateAsync(model);
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
                await _ppmpService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        #endregion

        [HttpGet("budgetyears")]
        public async Task<ActionResult<IList<int>>> GetBudgetYears()
        {
            try
            {
                var result = _ppmpService.GetBudgetYears();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest();
        }       
    }
}
