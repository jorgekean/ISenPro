using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class SsMopDetail
{
    public int MopDetailId { get; set; }

    public string? Description { get; set; }

    public bool? IsPercent { get; set; }

    public double? Value { get; set; }

    public bool IsActive { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int ModeOfProcurementId { get; set; }

    public virtual SsModeOfProcurement ModeOfProcurement { get; set; } = null!;
}
