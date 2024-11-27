using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.SystemSetup
{
    public class PurchasingTypeDto : BaseDto
    {
        public int? Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool WithCondition { get; set; }

        public decimal? MinimumAmount { get; set; }

        public decimal? MaximumAmount { get; set; }

       
    }
}
