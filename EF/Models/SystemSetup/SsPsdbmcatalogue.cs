using System;
using System.Collections.Generic;

namespace EF.Models.SystemSetup;

public partial class SsPsdbmcatalogue
{
    public int PsdbmcatalogueId { get; set; }

    public DateTime? CatalogueYear { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public decimal? UnitPrice { get; set; }

    public bool? IsCurrent { get; set; }

    public string? Remarks { get; set; }

    public string? Thumbnail { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int UnitOfMeasurementId { get; set; }

    public bool? IsOriginal { get; set; }

    public virtual SsUnitOfMeasurement UnitOfMeasurement { get; set; } = null!;
}
