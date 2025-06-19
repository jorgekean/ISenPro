using EF.Models;
using EF.Models.UserManagement;
using Service.Dto.SystemSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.Transaction
{
    public class PPMPDto : TransactionBaseDto
    {
        public int Id { get; set; }

        public string Ppmpno { get; set; } = null!;

        public short BudgetYear { get; set; }

        public string? Remarks { get; set; }

        public decimal? CatalogueAmount { get; set; }

        public decimal? SupplementaryAmount { get; set; }

        public decimal? ProjectAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? AdditionalInflationValue { get; set; }

        public decimal? AdditionalTenPercent { get; set; }

        public decimal? GrandTotalAmount { get; set; }        

        public int? RequestingOfficeId { get; set; }

        public string Status { get; set; } = null!;

        public IEnumerable<PPMPCatalogueDto> Ppmpcatalogues { get; set; } = new List<PPMPCatalogueDto>();

        public IEnumerable<PPMPSupplementariesDto> Ppmpsupplementaries { get; set; } = new List<PPMPSupplementariesDto>();
        public IEnumerable<PPMPProjectDto> PpmpProjects { get; set; } = new List<PPMPProjectDto>();

        public DepartmentDto? RequestingOffice { get; set; } = null!;


        public bool CanApprove { get; set; }
    }

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

    public class PPMPSupplementariesDto : TransactionBaseDto
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

        public int SupplementaryId { get; set; }

        public PPMPDto? Ppmp { get; set; } = null!;

        public SupplementaryCatalogueDto? Supplementary { get; set; } = null!;
    }

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
