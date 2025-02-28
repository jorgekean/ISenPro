using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ppmpcatalogue
{
    public int PpmpcatalogueId { get; set; }

    public int Ppmpid { get; set; }

    public string? Description { get; set; }

    public int FirstQuarter { get; set; }

    public int SecondQuarter { get; set; }

    public int ThirdQuarter { get; set; }

    public int FourthQuarter { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Amount { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedByUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedByUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int CatalogueId { get; set; }

    public virtual SsPsdbmcatalogue Catalogue { get; set; } = null!;

    public virtual Ppmp Ppmp { get; set; } = null!;
}
