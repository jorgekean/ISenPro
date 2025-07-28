using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class VPrPpmpPsdbmcatalogue
{
    public int? RequestingOfficeId { get; set; }

    public short BudgetYear { get; set; }

    public int PpmpcatalogueId { get; set; }

    public int Ppmpid { get; set; }

    public string? Description { get; set; }

    public int FirstQty { get; set; }

    public int SecondQty { get; set; }

    public int ThirdQty { get; set; }

    public int FourthQty { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Amount { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }

    public int CatalogueId { get; set; }

    public string? CatalogueCode { get; set; }

    public string MajorCategoryName { get; set; } = null!;

    public int UnitOfMeasurementId { get; set; }

    public string? UnitOfMeasurementCode { get; set; }

    public string ItemName { get; set; } = null!;

    public int ItemTypeId { get; set; }

    public string? AccountCodeDescription { get; set; }
}
