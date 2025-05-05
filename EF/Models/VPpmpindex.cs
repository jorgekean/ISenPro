using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class VPpmpindex
{
    public int Ppmpid { get; set; }

    public short BudgetYear { get; set; }

    public string Status { get; set; } = null!;

    public string Ppmpno { get; set; } = null!;

    public int? RequestingOfficeId { get; set; }

    public int CreatedByUserId { get; set; }

    public string? OfficeName { get; set; }

    public string PreparedBy { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }
}
