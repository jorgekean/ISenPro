using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class VSupplierIndex
{
    public int Id { get; set; }

    public string? CompanyName { get; set; }

    public string? IndustryName { get; set; }

    public string? EmailAddress { get; set; }

    public string? Address { get; set; }

    public string IsBlackListedStr { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }
}
