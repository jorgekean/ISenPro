using System;
using System.Collections.Generic;

namespace EF.Models.SystemSetup;

public partial class SsSupplier
{
    public int SupplierId { get; set; }

    public bool? Blacklist { get; set; }

    public string? CompanyName { get; set; }

    public string? Address { get; set; }

    public string? EmailAddress { get; set; }

    public string? Remarks { get; set; }

    public string? FaxNumber { get; set; }

    public string? Tin { get; set; }

    public int? Industry { get; set; }

    public bool IsActive { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? UpdatedByUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public long? RestoredByUserId { get; set; }

    public DateTime? RestoredDate { get; set; }

    public long? DeletedByUserId { get; set; }

    public DateTime? DeletedDate { get; set; }
}
