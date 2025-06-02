using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace Service
{
    using EF.Models;
    using global::Service.Transaction.Interface;
    using Microsoft.EntityFrameworkCore;
       using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    namespace Service
    {
        public class TransactionService: ITransactionService
        {
            protected readonly ISenProContext _context;            

            protected readonly IUserContext _userContext;

            public TransactionService(ISenProContext context, IUserContext userContext)
            {
                _context = context;                
                _userContext = userContext;
            }


            public virtual async Task<List<VTransactionHistory>> GetTransactionStatus(int transactionId, int pageId)
            {
                var result = await _context.VTransactionHistories.Where(x => x.TransactionId == transactionId && x.PageId == pageId)
                            .OrderByDescending(x => x.CreatedDate).ToListAsync();

                return result;
            }
        }
    }

}
