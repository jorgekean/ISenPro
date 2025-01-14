using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class ChangeTrackingTable
{
    public int ChangeId { get; set; }

    public DateTime Timestamp { get; set; }

    public string Operation { get; set; } = null!;

    public string? UserId { get; set; }

    public virtual ICollection<ChangeDetailsTable> ChangeDetailsTables { get; set; } = new List<ChangeDetailsTable>();
}
