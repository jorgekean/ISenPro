using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class UmRole
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public virtual ICollection<UmUserAccountRole> UmUserAccountRoles { get; set; } = new List<UmUserAccountRole>();
}
