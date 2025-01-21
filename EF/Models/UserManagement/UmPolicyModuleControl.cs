using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public partial class UmPolicyModuleControl
{
    public int PolicyModuleControlId { get; set; }

    public int PolicyId { get; set; }

    public bool IsChecked { get; set; }
    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int ModuleControlId { get; set; }

    public virtual UmModuleControl ModuleControl { get; set; } = null!;
}
