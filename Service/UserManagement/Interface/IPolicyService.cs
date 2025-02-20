using EF.Models.UserManagement;
using Service.Dto.UserManagement;
using EF.Models;

namespace Service.UserManagement.Interface
{
    public interface IPolicyService : IBaseService<UmPolicy, PolicyDto>
    {
        // Additional methods specific

        //Task<IEnumerable<PageDto>> GetAllPagesAsync();
        //Task<IEnumerable<ControlDto>> GetAllControlsAsync();
    }
}
