using API.Dto;
using Azure;
using EF.Models;
using EF.Models.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Dto.UserManagement;
using Service.UserManagement;
using Service.UserManagement.Interface;

namespace API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkFlowsController : ControllerBase
    {
        private readonly IWorkFlowService _workFlowService;
        private readonly IModuleService _moduleService;
        private readonly IWorkStepService _workStepService;
        private readonly ILogger<WorkFlowsController> _logger;

        public WorkFlowsController(IWorkFlowService workFlowService, IModuleService moduleService, IWorkStepService workStepService, ILogger<WorkFlowsController> logger)
        {
            _moduleService = moduleService;
            _workFlowService = workFlowService;
            _workStepService = workStepService;
            _logger = logger;
        }

        // GET: api/WorkFlows/Modules
        [HttpGet("Modules")]
        public async Task<ActionResult<PaginatedResponse<ModuleDto>>> GetModules([FromQuery] int workFlowId)
        {
            try
            {
                var response = await _workFlowService.GetTransactionAndMonitoringModules(workFlowId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/WorkFlows
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<VWorkFlowIndex>>> GetWorkFlows([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _workFlowService.GetComplexPagedAndFilteredAsync<VWorkFlowIndex>(pagingParameters);

                var response = new PaginatedResponse<VWorkFlowIndex>
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

        // GET: api/WorkFlows
        [HttpGet("WorkSteps")]
        public async Task<ActionResult<PaginatedResponse<WorkStepDto>>> GetWorkSteps([FromQuery] int workFlowId)
        {
            try
            {
                var paginatedResult = await _workFlowService.GetWorkSteps(workFlowId);

                var response = new PaginatedResponse<WorkStepDto>
                {
                    Items = paginatedResult,
                    TotalCount = paginatedResult.Count(),
                    PageNumber = 1,
                    PageSize = 100
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/WorkFlows
        [HttpPost("DeleteWorkStep")]
        public async Task<IActionResult> DeleteWorkStep(WorkStepDto workStep)
        {
            try
            {
                var workStepId = workStep.Id.HasValue ? workStep.Id.Value : 0;
                await _workStepService.DeleteAsync(workStepId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/WorkFlows
        [HttpPost("UpdateWorkStep")]
        public async Task<IActionResult> UpdateWorkStep(WorkStepDto workStep)
        {
            try
            {
                await _workStepService.UpdateAsync(workStep);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/WorkFlows
        [HttpPost("AddWorkStep")]
        public async Task<IActionResult> AddWorkStep(WorkStepDto workStep)
        {
            try
            {
                await _workStepService.AddAsync(workStep);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/WorkFlows/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkFlowDto>> GetWorkFlow(int id)
        {
            var umWorkFlow = await _workFlowService.GetByIdAsync(id);

            if (umWorkFlow == null)
            {
                return NotFound();
            }

            return umWorkFlow;
        }

        // PUT: api/WorkFlows/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkFlow(string id, WorkFlowDto umWorkFlow)
        {
            if (id != umWorkFlow.Code)
            {
                return BadRequest();
            }

            try
            {
                await _workFlowService.UpdateAsync(umWorkFlow);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await workFlowExists(id)))
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

        // POST: api/WorkFlows
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkFlowDto>> PostUmWorkFlow(WorkFlowDto umWorkFlow)
        {
            //_context.UmWorkFlows.Add(umWorkFlow);
            try
            {
                var entity = (UmWorkFlow) await _workFlowService.AddAsync(umWorkFlow);
                umWorkFlow.Id = entity.WorkflowId;
            }
            catch (DbUpdateException ex)
            {
                if (await workFlowExists(umWorkFlow.Code))
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

            return CreatedAtAction("GetWorkFlow", new { id = umWorkFlow.Code }, umWorkFlow);
        }

        //// DELETE: api/WorkFlows/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmWorkFlow(int id)
        {
            //    var workFlow = await _workFlowService.GetByIdAsync(id);
            //    if (workFlow == null)
            //    {
            //        return NotFound();
            //    }

            try
            {
                await _workFlowService.DeleteAsync(id);
            }
            catch (DbUpdateException ex)
            {
                //if (!(await workFlowExists(id)))
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

        private async Task<bool> workFlowExists(string id)
        {
            return (await _workFlowService.GetAllAsync()).Any(e => e.Code == id);
        }
    }
}
