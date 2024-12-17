using EF.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.UserManagement
{
    public class PageDto : BaseDto
    {
        public int? Id { get; set; }        
        public string PageName { get; set; } = null!;        

        public string? Description { get; set; }       
    }

    public class ControlDto : BaseDto
    {
        public int? Id { get; set; }
        public string ControlName { get; set; } = null!;

        public string? Description { get; set; }
    }
}
