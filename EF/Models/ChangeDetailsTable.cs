using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class ChangeDetailsTable
{
    public int ChangeId { get; set; }

    public string FieldName { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public virtual ChangeTrackingTable Change { get; set; } = null!;
}
