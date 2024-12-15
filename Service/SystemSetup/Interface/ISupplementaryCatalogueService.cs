using EF.Models;
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
    public interface ISupplementaryCatalogueService : IBaseService<SsSupplementaryCatalogue, SupplementaryCatalogueDto>
    {
        // Additional methods specific to ProductService

        Task<List<UnitOfMeasurementDto>> GetUnitOfMeasurements();
        Task<List<AccountCodeDto>> GetAccountCodes();
    }
}
