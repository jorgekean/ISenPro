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
    public class AccountCodesController : ControllerBase
    {
        private readonly IAccountCodeService _accountCodeservice;
        private readonly ILogger<AccountCodesController> _logger;

        public AccountCodesController(IAccountCodeService accountCodeservice, ILogger<AccountCodesController> logger)
        {
            _accountCodeservice = accountCodeservice;
            _logger = logger;
        }

        // GET: api/AccountCodes/ItemTypes
        [HttpGet("ItemTypes")]
        public async Task<ActionResult<PaginatedResponse<ItemTypeDto>>> GetItemTypes()
        {
            try
            {
                var response = await _accountCodeservice.GetItemTypes();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/AccountCodes
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<AccountCodeDto>>> GetUOMs([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var paginatedResult = await _accountCodeservice.GetPagedAndFilteredAsync(pagingParameters);

                var response = new PaginatedResponse<AccountCodeDto>
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


        // GET: api/AccountCodes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountCodeDto>> GetAccountCode(int id)
        {
            try
            {
                var umAccountCode = await _accountCodeservice.GetByIdAsync(id);

                if (umAccountCode == null)
                {
                    return NotFound();
                }

                return umAccountCode;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
            }


            return BadRequest();
        }

        // PUT: api/AccountCodes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountCode(string id, AccountCodeDto umAccountCode)
        {
            if (id != umAccountCode.Code)
            {
                return BadRequest();
            }

            try
            {
                await _accountCodeservice.UpdateAsync(umAccountCode);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!(await accountcodeExists(umAccountCode.Code)))
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

        // POST: api/AccountCodes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AccountCodeDto>> PostUmAccountCode(AccountCodeDto umAccountCode)
        {
            //_context.UmAccountCodes.Add(umAccountCode);
            try
            {
                await _accountCodeservice.AddAsync(umAccountCode);
            }
            catch (DbUpdateException ex)
            {
                if (await accountcodeExists(umAccountCode.Code))
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

            return CreatedAtAction("GetAccountCode", new { id = umAccountCode.Code }, umAccountCode);
        }

        //// DELETE: api/AccountCodes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUmAccountCode(int id)
        {

            try
            {
                await _accountCodeservice.DeleteAsync(id);
            }           
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        private async Task<bool> accountcodeExists(string code)
        {
            return (await _accountCodeservice.GetAllAsync()).Any(e => e.Code == code);
        }
    }
}
