using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ppmp
{
    public int Ppmpid { get; set; }

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

    public bool IsSubmitted { get; set; }

    public int? SubmittedByUserId { get; set; }

    public DateTime? SubmittedDate { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedByUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedByUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int? RequestingOfficeId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Ppmpcatalogue> Ppmpcatalogues { get; set; } = new List<Ppmpcatalogue>();

    public virtual ICollection<Ppmpsupplementary> Ppmpsupplementaries { get; set; } = new List<Ppmpsupplementary>();

    public virtual UmDepartment? RequestingOffice { get; set; }
}
