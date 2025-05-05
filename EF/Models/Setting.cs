using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Setting
{
    public string Type { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public string? AltDesc { get; set; }

    public bool Enabled { get; set; }
}
