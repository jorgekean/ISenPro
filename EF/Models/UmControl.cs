using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class UmControl
{
    public int ControlId { get; set; }

    public string ControlName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<UmModuleControl> UmModuleControls { get; set; } = new List<UmModuleControl>();
}
