using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.SystemSetup
{
    public class UnitOfMeasurementDto : BaseDto
    {
        public int? Id { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;      
       
    }
}
