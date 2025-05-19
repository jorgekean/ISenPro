using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class App
{
    public int Appid { get; set; }

    public string? Appno { get; set; }

    public short BudgetYear { get; set; }

    public decimal? TotalAmount { get; set; }

    public decimal? AdditionalInflationValue { get; set; }

    public decimal? AdditionalTenPercent { get; set; }

    public decimal? GrandTotal { get; set; }

    public string Status { get; set; } = null!;

    public decimal? AmendedTotalAmount { get; set; }

    public decimal? AmendedAdditionalTenPercent { get; set; }

    public decimal? AmendedGrandTotal { get; set; }

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

    public virtual ICollection<Appdetail> Appdetails { get; set; } = new List<Appdetail>();
}
