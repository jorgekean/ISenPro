using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.SystemSetup
{
    public class ItemTypeDto : BaseDto
    {
        public int? Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }        
    }
}
