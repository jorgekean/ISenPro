using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class UmUserAccount
{
    public int UserAccountId { get; set; }

    public string UserId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? ExpireDate { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int PersonId { get; set; }

    public int? RoleId { get; set; }

    public virtual UmPerson Person { get; set; } = null!;

    public virtual UmRole? Role { get; set; }

    public virtual ICollection<UmFilterCriterion> UmFilterCriteria { get; set; } = new List<UmFilterCriterion>();

    public virtual ICollection<UmWorkStepApprover> UmWorkStepApprovers { get; set; } = new List<UmWorkStepApprover>();
}
