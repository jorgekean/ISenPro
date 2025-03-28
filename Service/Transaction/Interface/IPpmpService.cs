using EF.Models;
using Service.Dto.Transaction;

namespace Service.Transaction.Interface
{

    public interface IPpmpService : IBaseService<Ppmp, PPMPDto>
    {
        // Additional methods specific to this service

        // report
        Task GenerateReport(int id);
        IList<int> GetBudgetYears();
    }
}
