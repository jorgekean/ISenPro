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
    public class SubCategoriesController : ControllerBase
    {
        private readonly ISubCategoryService _subCategorieservice;
        private readonly ILogger<SubCategoriesController> _logger;

        public SubCategoriesController(ISubCategoryService subCategorieservice, ILogger<SubCategoriesController> logger)
        {
            _subCategorieservice = subCategorieservice;
            _logger = logger;
        }

        //// GET: api/SubCategories/MajorCategories
        //[HttpGet("MajorCategories")]
        //public async Task<ActionResult<PaginatedResponse<MajorCategoryDto>>> GetMajorCategories()
        //{
        //    try
        //    {
        //        var response = await _subCategorieservice.GetMajorCategories();

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);

        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        // GET: api/SubCategories
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<SubCategoryDto>>> GetSubCategories([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _subCategorieservice.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<SubCategoryDto>
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


        // GET: api/SubCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubCategoryDto>> GetSubCategory(int id)
        {
            try
            {
                var umSubCategory = await _subCategorieservice.GetByIdAsync(id);

                if (umSubCategory == null)
                {
                    return NotFound();
                }

                return umSubCategory;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
            }


            return BadRequest();
        }

        // PUT: api/SubCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubCategory(string id, SubCategoryDto umSubCategory)
        {
            if (id != umSubCategory.Code)
            {
                return BadRequest();
            }

            try
            {
                await _subCategorieservice.UpdateAsync(umSubCategory);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await subcategoryExists(umSubCategory.Code)))
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

        // POST: api/SubCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SubCategoryDto>> PostUmSubCategory(SubCategoryDto umSubCategory)
        {
            //_context.UmSubCategories.Add(umSubCategory);
            try
            {
                await _subCategorieservice.AddAsync(umSubCategory);
            }
            catch (DbUpdateException ex)
            {
                if (await subcategoryExists(umSubCategory.Code))
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

            return CreatedAtAction("GetSubCategory", new { id = umSubCategory.Code }, umSubCategory);
        }

        //// DELETE: api/SubCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmSubCategory(int id)
        {

            try
            {
                await _subCategorieservice.DeleteAsync(id);
            }           
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        private async Task<bool> subcategoryExists(string code)
        {
            return (await _subCategorieservice.GetAllAsync()).Any(e => e.Code == code);
        }
    }
}
