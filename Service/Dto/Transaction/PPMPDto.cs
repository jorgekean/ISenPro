using EF.Models;
using EF.Models.UserManagement;
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
}
