using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class SsSupplierContactPerson
{
    public int SupplierContactPersonId { get; set; }

    public string? ContactPerson { get; set; }

    public string? ContactNumber { get; set; }

    public bool? IsActive { get; set; }

    public long? CreatedByUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? SupplierId { get; set; }

    public virtual SsSupplier? Supplier { get; set; }
}
