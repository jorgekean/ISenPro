using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public partial class UmWorkStepApprover
{
    public int WorkstepApproverId { get; set; }

    public int WorkstepId { get; set; }

    public int UserAccountId { get; set; }

    public bool IsActive { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual UmUserAccount UserAccount { get; set; } = null!;

    public virtual UmWorkStep Workstep { get; set; } = null!;
}
