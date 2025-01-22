using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public partial class UmWorkFlow
{
    public int WorkflowId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModuleId { get; set; }

    public virtual UmModule Module { get; set; } = null!;

    public virtual ICollection<UmWorkStep> UmWorkSteps { get; set; } = new List<UmWorkStep>();
}
