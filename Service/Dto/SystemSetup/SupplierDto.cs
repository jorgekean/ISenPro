using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.SystemSetup
{
    public class SupplierDto : BaseDto
    {
        public int Id { get; set; }

        public bool? Blacklist { get; set; }

        public string? CompanyName { get; set; }

        public string? Address { get; set; }

        public string? EmailAddress { get; set; }

        public string? Remarks { get; set; }

        public string? FaxNumber { get; set; }

        public string? Tin { get; set; }

        public int? Industry { get; set; }
    }
}
