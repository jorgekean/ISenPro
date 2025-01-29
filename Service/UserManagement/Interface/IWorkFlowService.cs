using EF.Models;
using EF.Models.UserManagement;
using Service.Dto.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UserManagement.Interface
{  
    public interface IWorkFlowService : IBaseService<UmWorkFlow, WorkFlowDto>
    {
        // Additional methods specific
        Task<List<WorkStepDto>> GetWorkSteps(int workFlowId);
        Task<List<ModuleDto>> GetTransactionAndMonitoringModules(int workFlowId);
    }
}
