using API.Dto;
using EF.Models.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.UserManagement.Interface;

namespace API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(IDepartmentService departmentService, ILogger<DepartmentsController> logger)
        {
            _departmentService = departmentService;
            _logger = logger;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<DepartmentDto>>> GetDepartments([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _departmentService.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<DepartmentDto>
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


        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
        {
            var umDepartment = await _departmentService.GetByIdAsync(id);

            if (umDepartment == null)
            {
                return NotFound();
            }

            return umDepartment;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(string id, DepartmentDto umDepartment)
        {
            if (id != umDepartment.Code)
            {
                return BadRequest();
            }

            try
            {
                await _departmentService.UpdateAsync(umDepartment);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await departmentExists(id)))
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

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> PostUmDepartment(DepartmentDto umDepartment)
        {
            //_context.UmDepartments.Add(umDepartment);
            try
            {
                await _departmentService.AddAsync(umDepartment);
            }
            catch (DbUpdateException ex)
            {
                if (await departmentExists(umDepartment.Code))
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

            return CreatedAtAction("GetDepartment", new { id = umDepartment.Code }, umDepartment);
        }

        //// DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmDepartment(int id)
        {
            //    var department = await _departmentService.GetByIdAsync(id);
            //    if (department == null)
            //    {
            //        return NotFound();
            //    }

            try
            {
                await _departmentService.DeleteAsync(id);
            }
            catch (DbUpdateException ex)
            {
                //if (!(await departmentExists(id)))
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

        private async Task<bool> departmentExists(string id)
        {
            return (await _departmentService.GetAllAsync()).Any(e => e.Code == id);
        }
    }
}
