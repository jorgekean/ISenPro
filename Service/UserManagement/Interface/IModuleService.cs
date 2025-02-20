using EF.Models.UserManagement;
using Service.Dto.UserManagement;
using EF.Models;

namespace Service.UserManagement.Interface
{
    public interface IModuleService : IBaseService<UmModule, ModuleDto>
    {
        // Additional methods specific
        Task<IEnumerable<PageDto>> GetAllPagesAsync();
        Task<IEnumerable<ControlDto>> GetAllControlsAsync();

        Task<List<ModuleDto>> GetTransactionAndMonitoringModules();
    }
}
