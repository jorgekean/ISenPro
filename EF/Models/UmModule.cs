using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class UmModule
{
    public int ModuleId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int ParentModuleId { get; set; }

    public int PageId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual UmPage Page { get; set; } = null!;

    public virtual ICollection<UmModuleControl> UmModuleControls { get; set; } = new List<UmModuleControl>();

    public virtual ICollection<UmWorkFlow> UmWorkFlows { get; set; } = new List<UmWorkFlow>();
}
