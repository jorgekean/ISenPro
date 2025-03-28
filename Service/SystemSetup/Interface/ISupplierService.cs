﻿
using EF.Models.UserManagement;
using Service.Dto.SystemSetup;
using Service.Dto.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF.Models;

namespace Service.SystemSetup.Interface
{
    public interface ISupplierService : IBaseService<SsSupplier, SupplierDto>
    {
       // Additional methods specific to Supplier Service
       Task<List<ReferenceTableDto>> GetIndustries();
    }
}
