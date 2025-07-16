using EF.Models;
using EF.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.Transaction
{
    public class PRDto : TransactionBaseDto
    {
        public int? Id { get; set; }

        public string? PrNumber { get; set; }

        public int RequestingOfficeId { get; set; }
        public string? RequestingOffice { get; set; }

        public decimal? TotalAmount { get; set; }


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

        public int? PrClassification { get; set; }

        public bool IsForDelivery { get; set; }

        public bool IsPartialSupply { get; set; }

        public bool IsPartialEquipment { get; set; }

        public bool HasSupplies { get; set; }

        public bool HasEquipments { get; set; }

        public bool IsConsolidated { get; set; }

        public bool IsCreatedBySupplyOfficer { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int? ConsolidatedBy { get; set; }

        public bool IsContract { get; set; }

        public int? ContractId { get; set; }

        public bool WithCanvassing { get; set; }

        public bool WithApr { get; set; }

        public bool IsPrSo { get; set; }

        public string? TempPrnumber { get; set; }

        public string? OtherRemarks { get; set; }

        public bool IsForRsqrfq { get; set; }

        public virtual IEnumerable<PurchaseRequestItemDto> PurchaseRequestItems { get; set; } = [];
        public virtual IEnumerable<PurchaseRequestItemDetailDto> PurchaseRequestItemDetails { get; set; } = [];// child of PurchaseRequestItems


        public bool CanApprove { get; set; }
    }

    public class PurchaseRequestItemDto : TransactionBaseDto
    {
        public int Id { get; set; }

        public int PurchaseRequestId { get; set; }

        public int? CatalogueId { get; set; }

        public string? ItemDescription { get; set; }

        public int? UnitOfMeasurementId { get; set; }
        public string? UnitOfMeasurement { get; set; }

        public int? RequestedQuantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? Amount { get; set; }

        public int RequestingOfficeId { get; set; }     

        public int? AvailableAt { get; set; }

        public int? ItemTypeId { get; set; }
        public string? ItemType { get; set; }

        public bool IsFailed { get; set; }

        public int? AmendedQuantity { get; set; }

        public decimal? AmendedUnitPrice { get; set; }

        public IEnumerable<PurchaseRequestItemDetailDto> PurchaseRequestItemDetails { get; set; } = new List<PurchaseRequestItemDetailDto>();
    }


    public class PurchaseRequestItemDetailDto: TransactionBaseDto
    {
        public int Id { get; set; }

        public int PurchaseRequestItemsId { get; set; }

        public string? ItemSpecification { get; set; }

        public int? RequestedQuantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public int? ItemTypeId { get; set; }
        public string? ItemType { get; set; }

        public int? UnitOfMeasureId { get; set; }
        public string? UnitOfMeasure { get; set; }

        //public virtual PurchaseRequestItem PurchaseRequestItems { get; set; } = null!;
    }

}
