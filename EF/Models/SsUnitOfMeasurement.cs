using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class SsUnitOfMeasurement
{
    public int UnitOfMeasurementId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<SsPsdbmcatalogue> SsPsdbmcatalogues { get; set; } = new List<SsPsdbmcatalogue>();

    public virtual ICollection<SsSupplementaryCatalogue> SsSupplementaryCatalogues { get; set; } = new List<SsSupplementaryCatalogue>();
}
