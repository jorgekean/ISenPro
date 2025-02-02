using EF.Models.SystemSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.SystemSetup
{
    public class SupplierContactPersonDto : BaseDto
    {
        public int Id { get; set; }

        public string? TempGuId { get; set; }

        public string? ContactPerson { get; set; }

        public string? ContactNumber { get; set; }

        public long? CreatedByUserId { get; set; }

        public int? SupplierId { get; set; }

        public virtual SupplierDto? Supplier { get; set; }
    }
}
