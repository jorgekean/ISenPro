using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class UmWorkStep
{
    public int WorkstepId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Sequence { get; set; }

    public bool? IsLastStep { get; set; }

    public int? RequiredApprover { get; set; }

    public bool? CanModify { get; set; }

    public int WorkflowId { get; set; }

    public bool IsActive { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<UmWorkStepApprover> UmWorkStepApprovers { get; set; } = new List<UmWorkStepApprover>();

    public virtual UmWorkFlow Workflow { get; set; } = null!;
}
