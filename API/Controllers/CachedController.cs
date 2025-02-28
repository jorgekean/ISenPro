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

        [HttpGet("psdbmcatalogues")]
        public async Task<ActionResult<IList<PSDBMCatalogueDto>>> GetPSDBCatalogues()
        {
            try
            {
                var result = await _cachedItems.PSDBMCatalogues;

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest();
        }

        [HttpGet("supplementarycatalogues")]
        public async Task<ActionResult<IList<PSDBMCatalogueDto>>> GetPSDBSupplementaryCatalogues()
        {
            try
            {
                var result = await _cachedItems.SupplementaryCatalogues;

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
