using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class PurchaseRequestItemDetail
{
    public int PurchaseRequestItemDetailsId { get; set; }

    public int PurchaseRequestItemsId { get; set; }

    public string? ItemSpecification { get; set; }

    public int? RequestedQuantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public int? ItemType { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedByUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedByUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int? UnitOfMeasure { get; set; }

    public virtual PurchaseRequestItem PurchaseRequestItems { get; set; } = null!;
}
