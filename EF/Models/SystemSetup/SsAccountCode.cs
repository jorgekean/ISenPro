using System;
using System.Collections.Generic;

namespace EF.Models.SystemSetup;

public partial class SsAccountCode
{
    public int AccountCodeId { get; set; }

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ItemTypeId { get; set; }

    public virtual SsItemType? ItemType { get; set; }
}
