using EF.Models;
using Service.Dto.Transaction;

namespace Service.Transaction.Interface
{

    public interface ITransactionService
    {
        Task<List<VTransactionHistory>> GetTransactionStatus(int transactionId, int pageId);
    }
}
