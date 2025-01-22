using API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Dto.SystemSetup;
using Service.Dto.UserManagement;
using Service.SystemSetup;
using Service.SystemSetup.Interface;
using Service.UserManagement.Interface;

namespace API.Controllers.SystemSetup
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignatoriesController : ControllerBase
    {
        private readonly ISignatoryService _signatoriesService;
        private readonly IModuleService _moduleService;
        private readonly ILogger<SignatoriesController> _logger;

        public SignatoriesController(ISignatoryService signatoriesService, IModuleService moduleService, ILogger<SignatoriesController> logger)
        {
            _moduleService = moduleService;
            _signatoriesService = signatoriesService;
            _logger = logger;
        }

        // GET: api/Signatories/Modules
        [HttpGet("Modules")]
        public async Task<ActionResult<PaginatedResponse<ModuleDto>>> GetModules()
        {
            try
            {
                var response = await _moduleService.GetTransactionAndMonitoringModules();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Signatories/ReferenceListDesignation
        [HttpGet("ReferenceListDesignation")]
        public async Task<ActionResult<ReferenceTableDto>> GetReferenceListDesignation(int refId)
        {
            try
            {
                var response = await _signatoriesService.GetListOfReference(5);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Signatories/GetReferenceListOffice
        [HttpGet("ReferenceListOffice")]
        public async Task<ActionResult<ReferenceTableDto>> GetReferenceListOffice()
        {
            try
            {
                var response = await _signatoriesService.GetListOfReference(6);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Signatories/GetReferenceListReportSection
        [HttpGet("ReferenceListReportSection")]
        public async Task<ActionResult<ReferenceTableDto>> GetReferenceListReportSection()
        {
            try
            {
                var response = await _signatoriesService.GetListOfReference(4);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Signatories
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<SignatoryDto>>> GetSignatories([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _signatoriesService.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<SignatoryDto>
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


        // GET: api/Signatories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SignatoryDto>> GetSignatories(int id)
        {
            try
            {
                var umSignatories = await _signatoriesService.GetByIdAsync(id);

                if (umSignatories == null)
                {
                    return NotFound();
                }

                return umSignatories;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
            }


            return BadRequest();
        }

        // PUT: api/Signatories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSignatories(string id, SignatoryDto umSignatories)
        {
            if (id != umSignatories.Id.ToString())
            {
                return BadRequest();
            }

            try
            {
                await _signatoriesService.UpdateAsync(umSignatories);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //if (!(await SignatoriesExists(umSignatories.CompanyName)))
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

        // POST: api/Signatories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SignatoryDto>> PostUmSignatories(SignatoryDto umSignatories)
        {
            //_context.UmSignatories.Add(umSignatories);
            try
            {
                await _signatoriesService.AddAsync(umSignatories);
            }
            catch (DbUpdateException ex)
            {
                //if (await SignatoriesExists(umSignatories.SignatoryDesignationId))
                //{
                //    return Conflict();
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

            return CreatedAtAction("GetSignatories", new { id = umSignatories.Id }, umSignatories);
        }

        //// DELETE: api/Signatories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmSignatories(int id)
        {

            try
            {
                await _signatoriesService.DeleteAsync(id);
            }           
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }
    }
}
