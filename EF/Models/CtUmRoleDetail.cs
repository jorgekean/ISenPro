using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class CtUmRoleDetail
{
    public int ChangeId { get; set; }

    public string FieldName { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public virtual CtUmRole Change { get; set; } = null!;
}
