using EF.Models;
using Service.Dto.SystemSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.Transaction
{
    public class PPMPCatalogueDto : TransactionBaseDto
    {
        public int Id { get; set; }

        public int Ppmpid { get; set; }

        public string? Description { get; set; }

        public int FirstQuarter { get; set; }

        public int SecondQuarter { get; set; }

        public int ThirdQuarter { get; set; }

        public int FourthQuarter { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Amount { get; set; }

        public string? Remarks { get; set; }       

        public int CatalogueId { get; set; }

        public PSDBMCatalogueDto? Catalogue { get; set; } = null!;

        public PPMPDto? Ppmp { get; set; } = null!;
        
    }
}
