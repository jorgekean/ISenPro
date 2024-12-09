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
    public class BureausController : ControllerBase
    {
        private readonly IBureauService _bureauService;
        private readonly ILogger<BureausController> _logger;

        public BureausController(IBureauService bureauService, ILogger<BureausController> logger)
        {
            _bureauService = bureauService;
            _logger = logger;
        }

        // GET: api/Bureaus
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<BureauDto>>> GetBureaus([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _bureauService.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<BureauDto>
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


        // GET: api/Bureaus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BureauDto>> GetBureau(int id)
        {
            var umBureau = await _bureauService.GetByIdAsync(id);

            if (umBureau == null)
            {
                return NotFound();
            }

            return umBureau;
        }

        // PUT: api/Bureaus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBureau(string id, BureauDto umBureau)
        {
            if (id != umBureau.Code)
            {
                return BadRequest();
            }

            try
            {
                await _bureauService.UpdateAsync(umBureau);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await bureauExists(id)))
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

        // POST: api/Bureaus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BureauDto>> PostUmBureau(BureauDto umBureau)
        {
            //_context.UmBureaus.Add(umBureau);
            try
            {
                await _bureauService.AddAsync(umBureau);
            }
            catch (DbUpdateException ex)
            {
                if (await bureauExists(umBureau.Code))
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

            return CreatedAtAction("GetBureau", new { id = umBureau.Code }, umBureau);
        }

        //// DELETE: api/Bureaus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmBureau(int id)
        {
            //    var bureau = await _bureauService.GetByIdAsync(id);
            //    if (bureau == null)
            //    {
            //        return NotFound();
            //    }

            try
            {
                await _bureauService.DeleteAsync(id);
            }
            catch (DbUpdateException ex)
            {
                //if (!(await bureauExists(id)))
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

        private async Task<bool> bureauExists(string id)
        {
            return (await _bureauService.GetAllAsync()).Any(e => e.Code == id);
        }
    }
}
