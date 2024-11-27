using System;
using System.Collections.Generic;

namespace EF.Models.SystemSetup;

public partial class SsPurchasingType
{
    public int PurchasingTypeId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool WithCondition { get; set; }

    public decimal? MinimumAmount { get; set; }

    public decimal? MaximumAmount { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }
}
