using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class VSignatoryIndex
{
    public int Id { get; set; }

    public string? ModuleName { get; set; }

    public string? SignatoryDesignation { get; set; }

    public string? SignatoryOffice { get; set; }

    public string? FullName { get; set; }

    public string? Office { get; set; }

    public int? Sequence { get; set; }

    public double? MinimumAmount { get; set; }

    public double? MaximumAmount { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }
}
