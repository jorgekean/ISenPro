using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class VParentModule
{
    public int ParentModuleId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
}
