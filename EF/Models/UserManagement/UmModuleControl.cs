using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public partial class UmModuleControl
{
    public int ModuleControlId { get; set; }

    public bool IsChecked { get; set; }

    public int ControlId { get; set; }

    public int ModuleId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual UmControl Control { get; set; } = null!;

    public virtual UmModule Module { get; set; } = null!;
}
