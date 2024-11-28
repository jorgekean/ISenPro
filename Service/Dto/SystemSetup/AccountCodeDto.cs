using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.SystemSetup
{
    public class AccountCodeDto : BaseDto
    {
        public int? Id { get; set; }

        public string Code { get; set; } = null!;

        public string? Description { get; set; }
             
        public int? ItemTypeId { get; set; }
        public string? ItemTypeName { get; set; }
    }
}
