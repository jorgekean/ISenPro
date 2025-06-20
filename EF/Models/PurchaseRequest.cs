using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class PurchaseRequest
{
    public int PurchaseRequestId { get; set; }

    public string? Prnumber { get; set; }

    public int RequestingOffice { get; set; }

    public decimal? TotalAmount { get; set; }

    public bool? IsSubmitted { get; set; }

    public short PurchasingType { get; set; }

    public int? PpmpId { get; set; }

    public string? PpmpNumber { get; set; }

    public short BudgetYear { get; set; }

    public string? Remarks { get; set; }

    public int? ModeOfProcurement { get; set; }

    public DateTime? DatePrepared { get; set; }

    public string? Purpose { get; set; }

    public bool? FirstQuarter { get; set; }

    public bool SecondQuarter { get; set; }

    public bool ThirdQuarter { get; set; }

    public bool FourthQuarter { get; set; }

    public bool IsRepeatOrder { get; set; }

    public bool WithoutPo { get; set; }

    public bool IsItemized { get; set; }

    public int? Supplier { get; set; }

    public int? Prkind { get; set; }

    public int? Prclassification { get; set; }

    public bool IsForDelivery { get; set; }

    public bool IsPartialSupply { get; set; }

    public bool IsPartialEquipment { get; set; }

    public bool HasSupplies { get; set; }

    public bool HasEquipments { get; set; }

    public DateTime? SubmittedDate { get; set; }

    public int? SubmittedBy { get; set; }

    public bool IsConsolidated { get; set; }

    public bool IsCreatedBySupplyOfficer { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public int? ConsolidatedBy { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedByUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DeletedByUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsContract { get; set; }

    public int? ContractId { get; set; }

    public bool WithCanvassing { get; set; }

    public bool WithApr { get; set; }

    public bool IsPrSo { get; set; }

    public string? TempPrnumber { get; set; }

    public string? OtherRemarks { get; set; }

    public bool IsForRsqrfq { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<PurchaseRequestItem> PurchaseRequestItems { get; set; } = new List<PurchaseRequestItem>();
}
