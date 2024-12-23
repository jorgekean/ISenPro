using EF.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.UserManagement
{
    public class UserAccountDto : BaseDto
    {
        public int? Id { get; set; }

        public string UserId { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateTime? ExpireDate { get; set; }

        public bool IsAdmin { get; set; }

        public int PersonId { get; set; }

        public int? RoleId { get; set; }

        public PersonDto? Person { get; set; }

        public RoleDto? Role { get; set; }
    }
}
