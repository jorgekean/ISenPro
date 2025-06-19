using EF.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.Transaction
{
    public class APPDto : TransactionBaseDto
    {        
        public int? Id { get; set; }

        public string? AppNo { get; set; }
        public short? BudgetYear { get; set; }
        public string? Status { get; set; } = string.Empty;

        public decimal? TotalCatalogueAmount { get; set; }
        public decimal? TotalSupplementaryAmount { get; set; }
        public decimal? TotalProjectAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? AdditionalInflationValue { get; set; }
        public decimal? AdditionalTenPercent { get; set; }
        public decimal? GrandTotalAmount { get; set; }

        public List<AppDetailsDto>? AppDetails { get; set; } = [];      

        public bool? CanApprove { get; set; }
    }

    public class AppDetailsDto
    {
        public int? Id { get; set; }
        public int? AppId { get; set; }
        public int? PpmpId { get; set; }
    }

    public class APPDetailsPPMPDto
    {
        public int PpmpId { get; set; }
        public int BudgetYear { get; set; }
        public string? PpmpNo { get; set; }
        public string? DateSubmitted { get; set; }
        public required string RequestingOffice { get; set; }
        public required string OfficeCode { get; set; }
    }

    public class APPCatalogueDto
    {
        public int PpmpCatalogueId { get; set; }
        public int PpmpId { get; set; }
        public required string ItemCode { get; set; }
        public required string ProductCategory { get; set; }
        public required string Description { get; set; }
        public int FirstQty { get; set; }
        public int SecondQty { get; set; }
        public int ThirdQty { get; set; }
        public int FourthQty { get; set; }
        public int TotalQty => FirstQty + SecondQty + ThirdQty + FourthQty;
        public required string RequestingOffice { get; set; }
        public required string UnitOfMeasure { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount => TotalQty * UnitPrice;
    }

    public class APPProjectItemDto
    {
        public int PpmpProjectId { get; set; }
        public int PpmpId { get; set; }
        public required string RequestingOffice { get; set; }
        public required string ProjectName { get; set; }
        public required string Description { get; set; }
        public int Quarter { get; set; }                
        public decimal Cost { get; set; }        
    }
}
