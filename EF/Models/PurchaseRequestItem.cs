using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class PurchaseRequestItem
{
    public int PurchaseRequestItemsId { get; set; }

    public int PurchaseRequestId { get; set; }

    public int? CatalogueId { get; set; }

    public string? ItemDescription { get; set; }

    public int? UnitOfMeasurement { get; set; }

    public int? RequestedQuantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? Amount { get; set; }

    public int RequestingOfficeId { get; set; }

    public int? AvailableAt { get; set; }

    public int? ItemType { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedByUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedByUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsFailed { get; set; }

    public int? AmendedQuantity { get; set; }

    public decimal? AmendedUnitPrice { get; set; }

    public virtual PurchaseRequest PurchaseRequest { get; set; } = null!;

    public virtual ICollection<PurchaseRequestItemDetail> PurchaseRequestItemDetails { get; set; } = new List<PurchaseRequestItemDetail>();
}

