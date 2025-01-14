using EF.Models.SystemSetup;
using EF.Models.UserManagement;
using Service.Dto.SystemSetup;
using Service.Dto.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.SystemSetup.Interface
{
    public interface ISignatoryService : IBaseService<SsSignatory, SignatoryDto>
    {
        // Additional methods specific to Signatory Service
        Task<List<ModuleDto>> GetModules();
        Task<List<ReferenceTableDto>> GetListOfReference(int refId);
    }
}
