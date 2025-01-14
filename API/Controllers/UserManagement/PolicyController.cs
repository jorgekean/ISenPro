using API.Dto;
using EF.Models.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Dto.UserManagement;
using Service.UserManagement.Interface;

namespace API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        private readonly IPolicyService _policySercvice;
        private readonly ILogger<PoliciesController> _logger;

        public PoliciesController(IPolicyService policieservice, ILogger<PoliciesController> logger)
        {
            _policySercvice = policieservice;
            _logger = logger;
        }

        // GET: api/Policies
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<PolicyDto>>> GetPolicies([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _policySercvice.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<PolicyDto>
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


        // GET: api/Policies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PolicyDto>> GetPolicy(int id)
        {
            var umRole = await _policySercvice.GetByIdAsync(id);

            if (umRole == null)
            {
                return NotFound();
            }

            return umRole;
        }

        // PUT: api/Policies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPolicy(string id, PolicyDto umRole)
        {
            if (id != umRole.Code)
            {
                return BadRequest();
            }

            try
            {
                await _policySercvice.UpdateAsync(umRole);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await roleExists(id)))
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

        // POST: api/Policies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PolicyDto>> PostUmRole(PolicyDto umRole)
        {
            //_context.UmPolicies.Add(umRole);
            try
            {
                await _policySercvice.AddAsync(umRole);
            }
            catch (DbUpdateException ex)
            {
                if (await roleExists(umRole.Code))
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

            return CreatedAtAction("GetRole", new { id = umRole.Code }, umRole);
        }

        //// DELETE: api/Policies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmRole(int id)
        {
            //    var policy = await _policySercvice.GetByIdAsync(id);
            //    if (policy == null)
            //    {
            //        return NotFound();
            //    }

            try
            {
                await _policySercvice.DeleteAsync(id);
            }
            catch (DbUpdateException ex)
            {
                //if (!(await roleExists(id)))
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

        private async Task<bool> roleExists(string id)
        {
            return (await _policySercvice.GetAllAsync()).Any(e => e.Code == id);
        }
    }
}
