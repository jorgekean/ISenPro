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

        Task<List<APPDetailsPPMPDto>> GetOfficesNoPPMPs(short budgetYear);
        Task<List<APPDetailsPPMPDto>> GetOfficesWithApprovedPPMPs(short budgetYear);
        Task<List<APPDetailsPPMPDto>> GetOfficesWithSavedPPMPs(short budgetYear);
        Task<List<APPCatalogueDto>> ViewConsolidated(short budgetYear);
        Task<List<APPCatalogueDto>> ViewConsolidatedSuppItems(short budgetYear);
        Task<List<APPProjectItemDto>> ViewConsolidatedProjectItems(short budgetYear);
    }
}
