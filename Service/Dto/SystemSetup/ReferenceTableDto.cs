using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.SystemSetup
{
    public class ReferenceTableDto : BaseDto
    {
        public int? Id { get; set; }

        public int? RefTableId { get; set; }

        public string? RefTableName { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Code { get; set; }
    }
}
