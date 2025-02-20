using System;
using System.Collections.Generic;

namespace EF.Models.SystemSetup;

public partial class SsItemStatus
{
    public int ItemStatusId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<SsSubStatus> SsSubStatuses { get; set; } = new List<SsSubStatus>();
}
