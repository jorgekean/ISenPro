using EF.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.UserManagement
{
    public class ModuleControlDto : BaseDto
    {
        public int? Id { get; set; }

        public bool IsChecked { get; set; }

        public int ControlId { get; set; }

        public int? ModuleId { get; set; }       

        public string? ControlName { get; set; }

        public string? ModuleName { get; set; }
    }
}
