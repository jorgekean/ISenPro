﻿using EF.Models;
using EF.Models.UserManagement;
using Service.Dto.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UserManagement.Interface
{  
    public interface IBureauService : IBaseService<UmBureau, BureauDto>
    {
        // Additional methods specific
    }
}