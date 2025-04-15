using API.Dto;
using EF.Models;
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
    public class UserAccountsController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly ILogger<UserAccountsController> _logger;

        public UserAccountsController(IUserAccountService userAccountService, ILogger<UserAccountsController> logger)
        {
            _userAccountService = userAccountService;
            _logger = logger;
        }

        // GET: api/UserAccounts
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<VUserAccountIndex>>> GetUserAccounts([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _userAccountService.GetComplexPagedAndFilteredAsync<VUserAccountIndex>(pagingParameters);

                var response = new PaginatedResponse<VUserAccountIndex>
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


        // GET: api/UserAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAccountDto>> GetUserAccount(int id)
        {
            var umUserAccount = await _userAccountService.GetByIdAsync(id);

            if (umUserAccount == null)
            {
                return NotFound();
            }

            return umUserAccount;
        }

        // PUT: api/UserAccounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAccount(int id, UserAccountDto umUserAccount)
        {
            if (id != umUserAccount.Id)
            {
                return BadRequest();
            }

            try
            {
                await _userAccountService.UpdateAsync(umUserAccount);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //    if (!(await useraccountExists(id)))
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

        // POST: api/UserAccounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserAccountDto>> PostUmUserAccount(UserAccountDto umUserAccount)
        {
            //_context.UmUserAccounts.Add(umUserAccount);
            try
            {
                await _userAccountService.AddAsync(umUserAccount);
            }
            catch (DbUpdateException ex)
            {
                //if (await useraccountExists(umUserAccount.Code))
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

            return CreatedAtAction("GetUserAccount", new { id = umUserAccount.UserId }, umUserAccount);
        }

        //// DELETE: api/UserAccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmUserAccount(int id)
        {           

            try
            {
                await _userAccountService.DeleteAsync(id);
            }
            catch (DbUpdateException ex)
            {
                //if (!(await useraccountExists(id)))
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

        //private async Task<bool> useraccountExists(string id)
        //{
        //    return (await _userAccountService.GetAllAsync()).Any(e => e.Code == id);
        //}
    }
}
