﻿using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class UmRole
{
    public int RoleId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public virtual ICollection<UmFilterCriterion> UmFilterCriteria { get; set; } = new List<UmFilterCriterion>();

    public virtual ICollection<UmPolicyRole> UmPolicyRoles { get; set; } = new List<UmPolicyRole>();

    public virtual ICollection<UmUserAccount> UmUserAccounts { get; set; } = new List<UmUserAccount>();
}
