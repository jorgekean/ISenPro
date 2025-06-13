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
        // create properties for below
        //    appNo: string;
        //budgetYear: number;
        //submittedDate?: string;
        //submittedBy?: number;
        //status: string;

        //totalCatalogueAmount?: number;
        //totalSupplementaryAmount?: number;
        //totalProjectAmount?: number;
        //totalAmount?: number;
        //additionalInflationValue?: number;
        //additionalTenPercent?: number;
        //grandTotalAmount?: number;

        //appDetails: Record<number, APPDetailsModel>;

        //officesWithApprovedPPMPs: Record<number, OfficeModel>;
        //officesWithASavedPPMPs: Record<number, OfficeModel>;
        //officesWithOutPPMPs: Record<number, OfficeModel>;
        public int Id { get; set; }

        public string? AppNo { get; set; }
        public short BudgetYear { get; set; }
        public string Status { get; set; } = string.Empty;

        public decimal? TotalCatalogueAmount { get; set; }
        public decimal? TotalSupplementaryAmount { get; set; }
        public decimal? TotalProjectAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? AdditionalInflationValue { get; set; }
        public decimal? AdditionalTenPercent { get; set; }
        public decimal? GrandTotalAmount { get; set; }

        //public Dictionary<int, APPDetailsModel> AppDetails { get; set; } = new Dictionary<int, APPDetailsModel>();

        public List<APPDetailsPPMPDto> OfficesWithApprovedPPMPs { get; set; } = [];
        public List<APPDetailsPPMPDto> OfficesWithASavedPPMPs { get; set; } = [];
        public List<APPDetailsPPMPDto> OfficesWithOutPPMPs { get; set; } = [];

        public bool CanApprove { get; set; }
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
