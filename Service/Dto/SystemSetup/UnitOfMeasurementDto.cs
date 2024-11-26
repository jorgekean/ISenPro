using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.SystemSetup
{
    public class UnitOfMeasurementDto
    {
        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;      

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }


        public string CreatedDateStr { get { return CreatedDate.ToString("MM/dd/yyyy"); } }        
    }
}
