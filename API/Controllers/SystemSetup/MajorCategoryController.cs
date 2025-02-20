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
    public class MajorCategoriesController : ControllerBase
    {
        private readonly IMajorCategoryService _majorCategorieservice;
        private readonly ILogger<MajorCategoriesController> _logger;

        public MajorCategoriesController(IMajorCategoryService majorCategorieservice, ILogger<MajorCategoriesController> logger)
        {
            _majorCategorieservice = majorCategorieservice;
            _logger = logger;
        }

        // GET: api/MajorCategories/AccountCodes
        [HttpGet("AccountCodes")]
        public async Task<ActionResult<PaginatedResponse<AccountCodeDto>>> GetItemTypes()
        {
            try
            {
                var response = await _majorCategorieservice.GetAccountCodes();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/MajorCategories
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<MajorCategoryDto>>> GetMajorCategories([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _majorCategorieservice.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<MajorCategoryDto>
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


        // GET: api/MajorCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MajorCategoryDto>> GetMajorCategory(int id)
        {
            try
            {
                var umMajorCategory = await _majorCategorieservice.GetByIdAsync(id);

                if (umMajorCategory == null)
                {
                    return NotFound();
                }

                return umMajorCategory;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
            }


            return BadRequest();
        }

        // PUT: api/MajorCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMajorCategory(string id, MajorCategoryDto umMajorCategory)
        {
            if (id != umMajorCategory.Code)
            {
                return BadRequest();
            }

            try
            {
                await _majorCategorieservice.UpdateAsync(umMajorCategory);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await majorcategoryExists(umMajorCategory.Code)))
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

        // POST: api/MajorCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MajorCategoryDto>> PostUmMajorCategory(MajorCategoryDto umMajorCategory)
        {
            //_context.UmMajorCategories.Add(umMajorCategory);
            try
            {
                await _majorCategorieservice.AddAsync(umMajorCategory);
            }
            catch (DbUpdateException ex)
            {
                if (await majorcategoryExists(umMajorCategory.Code))
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

            return CreatedAtAction("GetMajorCategory", new { id = umMajorCategory.Code }, umMajorCategory);
        }

        //// DELETE: api/MajorCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmMajorCategory(int id)
        {

            try
            {
                await _majorCategorieservice.DeleteAsync(id);
            }           
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        private async Task<bool> majorcategoryExists(string code)
        {
            return (await _majorCategorieservice.GetAllAsync()).Any(e => e.Code == code);
        }
    }
}
