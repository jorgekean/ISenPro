using API.Controllers.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Cache;
using Service.Dto.SystemSetup;
using Service.Transaction.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CachedController : ControllerBase
    {
        private readonly CachedItems _cachedItems;

        private readonly ILogger<CachedController> _logger;
        public CachedController(ILogger<CachedController> logger, CachedItems cachedItems)
        {
            _cachedItems = cachedItems;
            _logger = logger;
        }

        [HttpGet("psdbmcatalogues/{year}")]
        public async Task<ActionResult<IList<PSDBMCatalogueDto>>> GetPSDBCatalogues(int year)
        {
            try
            {
                var result = await _cachedItems.GetPSDBMCatalogues(year);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest();
        }

        [HttpGet("supplementarycatalogues/{year}")]
        public async Task<ActionResult<IList<PSDBMCatalogueDto>>> GetPSDBSupplementaryCatalogues(int year)
        {
            try
            {
                var result = await _cachedItems.GetSupplementaryCatalogues(year);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest();
        }

        [HttpGet("accountcodes")]
        public async Task<ActionResult<IList<PSDBMCatalogueDto>>> GetAccountCodes()
        {
            try
            {
                var result = await _cachedItems.AccountCodes;

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest();
        }
    }
}
