using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class VPsdbmcatalogue
{
    public string ItemType { get; set; } = null!;

    public string? ItemCode { get; set; }

    public string Productcategory { get; set; } = null!;

    public string AccountCode { get; set; } = null!;

    public string? UnitOfMeasurement { get; set; }

    public string? Description { get; set; }

    public decimal? UnitPrice { get; set; }
}
