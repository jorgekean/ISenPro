using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.UserManagement
{
    public class RoleDto : BaseDto
    {
        public int? Id { get; set; }
        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
