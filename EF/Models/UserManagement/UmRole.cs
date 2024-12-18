using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public partial class UmRole
{
    public int RoleId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public virtual ICollection<UmUserAccount> UmUserAccounts { get; set; } = new List<UmUserAccount>();
}
