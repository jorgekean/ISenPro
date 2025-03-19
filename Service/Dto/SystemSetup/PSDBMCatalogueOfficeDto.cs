
using EF.Models;
using EF.Models.UserManagement;
using System;
using System.Collections.Generic;

namespace Service.Dto.SystemSetup
{
    public partial class PSDBMCatalogueOfficeDto : BaseDto
    {
        public int Id { get; set; }

        public int? DepartmentId { get; set; }

        public int? PSDBMCatalogueId { get; set; }
        
        public virtual DepartmentDto? Department { get; set; } = null!;

        public virtual PSDBMCatalogueDto? Psdbmcatalogue { get; set; } = null!;
    }
}