using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class VAppPpmpProjectItem
{
    public short BudgetYear { get; set; }

    public string? RequestingOffice { get; set; }

    public int PpmpprojectId { get; set; }

    public int Ppmpid { get; set; }

    public string? ProjectName { get; set; }

    public string? Description { get; set; }

    public int Quarter { get; set; }

    public decimal Cost { get; set; }

    public string? ProjectStatus { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedByUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedByUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int? AccountCodeId { get; set; }
}
