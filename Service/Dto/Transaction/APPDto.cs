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

        public List<DepartmentDto> OfficesWithApprovedPPMPs { get; set; } = [];
        public List<DepartmentDto> OfficesWithASavedPPMPs { get; set; } = [];
        public List<DepartmentDto> OfficesWithOutPPMPs { get; set; } = [];

        public bool CanApprove { get; set; }
    }
}
