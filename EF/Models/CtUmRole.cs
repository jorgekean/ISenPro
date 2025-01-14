using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class CtUmRole
{
    public int ChangeId { get; set; }

    public DateTime Timestamp { get; set; }

    public string Operation { get; set; } = null!;

    public string? UserId { get; set; }

    public virtual ICollection<CtUmRoleDetail> CtUmRoleDetails { get; set; } = new List<CtUmRoleDetail>();
}
