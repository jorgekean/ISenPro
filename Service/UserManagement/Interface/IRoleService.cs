using EF.Models;
using Service.Dto.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UserManagement.Interface
{  
    public interface IRoleService : IBaseService<UmRole, RoleDto>
    {
        // Additional methods specific to ProductService
    }
}
