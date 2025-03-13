using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class VRoleModuleControl
{
    public int ModuleControlId { get; set; }

    public bool IsChecked { get; set; }

    public int ControlId { get; set; }

    public int ModuleId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? ModuleName { get; set; }

    public string ParentModuleName { get; set; } = null!;

    public int RoleId { get; set; }
}
