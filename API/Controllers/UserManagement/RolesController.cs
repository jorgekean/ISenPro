using API.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Dto.UserManagement;
using Service.UserManagement.Interface;

namespace API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;
      
        public RolesController(IRoleService roleService, ILogger<RolesController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        // GET: api/Roles        
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<RoleDto>>> GetRoles([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _roleService.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<RoleDto>
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


        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(int id)
        {
            var umRole = await _roleService.GetByIdAsync(id);

            if (umRole == null)
            {
                return NotFound();
            }

            return umRole;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(string id, RoleDto umRole)
        {
            if (id != umRole.Code)
            {
                return BadRequest();
            }

            try
            {
                await _roleService.UpdateAsync(umRole);
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

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoleDto>> PostUmRole(RoleDto umRole)
        {
            //_context.UmRoles.Add(umRole);
            try
            {
                await _roleService.AddAsync(umRole);
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

        //// DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmRole(int id)
        {
            //    var role = await _roleService.GetByIdAsync(id);
            //    if (role == null)
            //    {
            //        return NotFound();
            //    }

            try
            {
                await _roleService.DeleteAsync(id);
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
            return (await _roleService.GetAllAsync()).Any(e => e.Code == id);
        }
    }
}
