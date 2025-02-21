using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class SsModeOfProcurement
{
    public int ModeOfProcurementId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? WithCondition { get; set; }

    public double? MinimumAmount { get; set; }

    public double? MaximumAmount { get; set; }

    public bool IsActive { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<SsMopDetail> SsMopDetails { get; set; } = new List<SsMopDetail>();
}
