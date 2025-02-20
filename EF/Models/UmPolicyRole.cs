using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class UmPolicyRole
{
    public int PolicyRoleId { get; set; }

    public int PolicyId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int RoleId { get; set; }

    public virtual UmRole Role { get; set; } = null!;
}
