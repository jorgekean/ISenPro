using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class VWorkFlowIndex
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? ModuleName { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public long CreatedByUserId { get; set; }
}
