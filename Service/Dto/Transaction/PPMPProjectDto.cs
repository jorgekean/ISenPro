using EF.Models;
using Service.Dto.SystemSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.Transaction
{
    public class PPMPProjectDto : TransactionBaseDto
    {
        public int Id { get; set; }

        public int Ppmpid { get; set; }

        public string? ProjectName { get; set; }

        public string? Description { get; set; }

        public int Quarter { get; set; }

        public decimal Cost { get; set; }

        public string? ProjectStatus { get; set; }        
        public AccountCodeDto? AccountCode { get; set; } = null!;

        public PPMPDto? Ppmp { get; set; } = null!;
    }

}
