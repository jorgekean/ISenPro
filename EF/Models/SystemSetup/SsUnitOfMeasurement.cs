using System;
using System.Collections.Generic;

namespace EF.Models.SystemSetup;

public partial class SsUnitOfMeasurement
{
    public int UnitOfMeasurementId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }
}
