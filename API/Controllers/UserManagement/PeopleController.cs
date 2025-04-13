using API.Dto;
using EF.Models;
using EF.Models.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.UserManagement.Interface;

namespace API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly ILogger<PeopleController> _logger;

        public PeopleController(IPersonService personService, ILogger<PeopleController> logger)
        {
            _personService = personService;
            _logger = logger;
        }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<VPersonIndex>>> GetPeople([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _personService.GetComplexPagedAndFilteredAsync<VPersonIndex>(pagingParameters);
                var empStatuses = paginatedResult.Data.Select(x => x.EmployeeStatusLabel).ToList();

                var response = new PaginatedResponse<VPersonIndex>
                {
                    Items = paginatedResult.Data.OrderBy(x => x.FullName),
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


        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDto>> GetPerson(int id)
        {
            var umPerson = await _personService.GetByIdAsync(id);

            if (umPerson == null)
            {
                return NotFound();
            }

            return umPerson;
        }

        // PUT: api/People/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, PersonDto umPerson)
        {
            if (id != umPerson.Id)
            {
                return BadRequest();
            }

            try
            {
                await _personService.UpdateAsync(umPerson);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //    if (!(await personExists(id)))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        _logger.LogError(ex, ex.Message);
                //    }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonDto>> PostUmPerson(PersonDto umPerson)
        {
            //_context.UmPeople.Add(umPerson);
            try
            {
                await _personService.AddAsync(umPerson);
            }
            catch (DbUpdateException ex)
            {
                //if (await personExists(umPerson.Code))
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

            return CreatedAtAction("GetPerson", new { id = umPerson.LastName }, umPerson);
        }

        //// DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmPerson(int id)
        {
            //    var person = await _personservice.GetByIdAsync(id);
            //    if (person == null)
            //    {
            //        return NotFound();
            //    }

            try
            {
                await _personService.DeleteAsync(id);
            }
            catch (DbUpdateException ex)
            {
                //if (!(await personExists(id)))
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

        //private async Task<bool> personExists(string id)
        //{
        //    return (await _personService.GetAllAsync()).Any(e => e.Code == id);
        //}
    }
}
