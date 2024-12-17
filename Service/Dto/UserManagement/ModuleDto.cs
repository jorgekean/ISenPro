using EF.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.UserManagement
{
    public class ModuleDto : BaseDto
    {
        public int? Id { get; set; }

        public string Code { get; set; } = null!;

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int ParentModuleId { get; set; }

        public int PageId { get; set; }

        public string? PageName { get; set; }

        public virtual IEnumerable<ModuleControlDto> ModuleControls { get; set; } = new List<ModuleControlDto>();
    }    
}
