using API.Dto;
using EF.Models.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Dto.UserManagement;
using Service.Enums;
using Service.Helpers;
using Service.UserManagement.Interface;
using EF.Models;

namespace API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        private readonly ILogger<ModulesController> _logger;

        public ModulesController(IModuleService moduleService, ILogger<ModulesController> logger)
        {
            _moduleService = moduleService;
            _logger = logger;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<ModuleDto>>> GetModules([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _moduleService.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<ModuleDto>
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


        // GET: api/Modules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleDto>> GetModule(int id)
        {
            var umModule = await _moduleService.GetByIdAsync(id);

            if (umModule == null)
            {
                return NotFound();
            }

            return umModule;
        }

        // PUT: api/Modules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(string id, ModuleDto umModule)
        {
            if (id != umModule.Code)
            {
                return BadRequest();
            }

            try
            {
                await _moduleService.UpdateAsync(umModule);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await moduleExists(id)))
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

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ModuleDto>> PostUmModule(ModuleDto umModule)
        {
            //_context.UmModules.Add(umModule);
            try
            {
                await _moduleService.AddAsync(umModule);
            }
            catch (DbUpdateException ex)
            {
                if (await moduleExists(umModule.Code))
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

            return CreatedAtAction("GetModule", new { id = umModule.Code }, umModule);
        }

        //// DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmModule(int id)
        {
            //    var module = await _moduleService.GetByIdAsync(id);
            //    if (module == null)
            //    {
            //        return NotFound();
            //    }

            try
            {
                await _moduleService.DeleteAsync(id);
            }
            catch (DbUpdateException ex)
            {
                //if (!(await moduleExists(id)))
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

        // GET: api/Modules/pages
        [HttpGet("pages")]
        public async Task<ActionResult<IEnumerable<PageDto>>> GetPages()
        {
            try
            {
                var pages = await _moduleService.GetAllPagesAsync();

                if (pages == null || !pages.Any())
                {
                    return NotFound();
                }

                var response = new PaginatedResponse<PageDto>
                {
                    Items = pages
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Modules/controls
        [HttpGet("controls")]
        public async Task<ActionResult<IEnumerable<ControlDto>>> GetControls()
        {
            try
            {
                var controls = await _moduleService.GetAllControlsAsync();

                if (controls == null || !controls.Any())
                {
                    return NotFound();
                }

                var response = new PaginatedResponse<ControlDto>
                {
                    Items = controls
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Modules/parentmodules
        [HttpGet("parentmodules")]
        public ActionResult<IEnumerable<ControlDto>> GetParentModules()
        {
            try
            {
                var enumList = EnumHelper.GetEnumList<ParentModule>();

                // Transform the enum list to match the desired response structure
                var list = enumList.Select(e => new
                {
                    Id = e.Value,
                    PageName = e.Description
                }).ToList();

                var response = new PaginatedResponse<object>
                {
                    Items = list
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }


        private async Task<bool> moduleExists(string id)
        {
            return (await _moduleService.GetAllAsync()).Any(e => e.Code == id);
        }
    }
}
