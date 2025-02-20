using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class UmPage
{
    public int PageId { get; set; }

    public string PageName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<UmModule> UmModules { get; set; } = new List<UmModule>();
}
