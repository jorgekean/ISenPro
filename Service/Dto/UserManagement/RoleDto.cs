using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.UserManagement
{
    public class RoleDto
    {
        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }


        public string CreatedDateStr => CreatedDate.ToString("d", CultureInfo.InvariantCulture);
    }
}
