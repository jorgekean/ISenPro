using EF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Transaction.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionService _transactionService;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        // GET: api/Transaction/History
        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<VTransactionHistory>>> GetTransactionHistory(
            [FromQuery] int transactionId = 0,
            [FromQuery] int pageId = 25)
        {
            try
            {

                // Fetch transaction history
                var transactions = await _transactionService.GetTransactionStatus(transactionId, pageId);               

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching transaction history");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
            }
        }
    }
}
