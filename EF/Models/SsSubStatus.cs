using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class SsSubStatus
{
    public int SubStatusId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int ItemStatusId { get; set; }

    public virtual SsItemStatus ItemStatus { get; set; } = null!;
}
