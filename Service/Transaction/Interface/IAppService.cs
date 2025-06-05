using EF.Models;
using Service.Dto.Transaction;

namespace Service.Transaction.Interface
{

    public interface IAppService : IBaseService<App, APPDto>
    {
        // Additional methods specific to this service

        // report
        Task GenerateReport(int id);
        IList<int> GetBudgetYears();

        Task<List<APPDetailsPPMPDto>> GetOfficeNoPPMPs(short budgetYear);
    }
}
