using EF.Models;
using EF.Models.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using Service.Cache;
using Service.Dto.SystemSetup;
using Service.Dto.Transaction;
using Service.Dto.UserManagement;
using Service.Reports;
using Service.Reports.Transactions;
using Service.Service;
using Service.SystemSetup.Interface;
using Service.Transaction.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Service.Transaction
{
    public class PrService : BaseService<PurchaseRequest, PRDto>, IPrService
    {
        private readonly CachedItems _cachedItems;

        public PrService(ISenProContext context, IUserContext userContext,
            CachedItems cachedItems) : base(context, userContext)
        {
            _cachedItems = cachedItems;
        }

        #region CRUD
        //protected override IQueryable<T> ApplySearchFilter<T>(IQueryable<T> query, string searchQuery)
        //{
        //    // If the type is MyEntity, cast the query and apply filtering.
        //    if (typeof(T) == typeof(VPpmpindex))
        //    {
        //        var typedQuery = query as IQueryable<VPpmpindex>;
        //        if (!string.IsNullOrWhiteSpace(searchQuery))
        //        {
        //            typedQuery = typedQuery.Where(p => new[] { p.BudgetYear.ToString(), p.Remarks, p.Status, p.Ppmpno, p.OfficeName, p.PreparedBy }
        //                     .Any(value => value != null && value.Contains(searchQuery)));
        //        }

        //        // Cast back to IQueryable<T> and return
        //        return typedQuery as IQueryable<T>;
        //    }

        //    // Otherwise, for any other type, you can either return the unfiltered query or add your own logic.
        //    return query;
        //}

        //public override IQueryable<T> ApplyFilterCriteria<T>(IQueryable<T> query)
        //{
        //    if (typeof(T) == typeof(VPpmpindex))
        //    {
        //        var userId = _userContext.UserId;
        //        var isAdmin = _userContext.IsAdmin;
        //        var parentModule = 1;

        //        var typedQuery = (IQueryable<VPpmpindex>)query;

        //        // This assumes you've registered the SQL function in your DbContext
        //        typedQuery = typedQuery.Where(p =>
        //            _context.ApplyTransactionFilters(
        //                userId,
        //                p.RequestingOfficeId,
        //                p.CreatedByUserId,
        //                p.Status,
        //                isAdmin,
        //                parentModule
        //            ));

        //        return (IQueryable<T>)typedQuery;
        //    }

        //    return query;
        //}
        //

        protected override IQueryable<PurchaseRequest> IncludeNavigationProperties(IQueryable<PurchaseRequest> query)
        {
            return query.Include(s => s.PurchaseRequestItems);
        }

        public override async Task<PRDto> GetByIdAsync(int id)
        {
            var model = await base.GetByIdAsync(id);

            //var officeModel = _context.UmDepartments.Find(model.RequestingOfficeId.GetValueOrDefault());            

            //var ppmpView = _context.VPpmpindices.FirstOrDefault(x => x.Ppmpid == id);
            //model.CreatedByStr = ppmpView?.PreparedBy;
            //model.RequestingOffice = ppmpView != null ? new DepartmentDto
            //{
            //    Id = ppmpView.RequestingOfficeId,
            //    Name = ppmpView.OfficeName,
            //    Code = ""
            //} : null;

            var userPermission = await _context.GetUserTransactionPermissionsAsync(id, _userContext.UserId, 25);

            model.UserTransactionPermissions = userPermission;
            model.CanApprove = model.IsSubmitted && userPermission != null && userPermission.CanApprove;

            return model;
        }

        public override async Task<object> AddAsync(PRDto dto)
        {
            dto.TempPrnumber = GenerateTempPrNo(dto.BudgetYear);

            return await base.AddAsync(dto);
        }

        // Expect to received the updated/added ppmpcatalogues and ppmpsupplementaries ONLY
        public override async Task UpdateAsync(PRDto dto)
        {
            var existingEntity = await _dbSet.FindAsync(dto.Id.GetValueOrDefault());

            if (existingEntity == null)
            {
                return;
            }

            var entity = MapToEntity(existingEntity, dto);

            #region Pr Items
            // Get a list of IDs from the DTO for records that are NOT new.
            var existingIds = dto.PurchaseRequestItems
                                 .Where(item => item.Id != 0)
                                 .Select(item => item.Id)
                                 .ToList();

            // Load all existing PrItems in one go.
            var existingPrItems = await _context.PurchaseRequestItems
                .Where(c => existingIds.Contains(c.PurchaseRequestItemsId))
                .ToDictionaryAsync(c => c.PurchaseRequestItemsId);

            foreach (var item in dto.PurchaseRequestItems)
            {
                switch (item.Status?.ToLower())
                {
                    case "modified":
                        if (existingPrItems.TryGetValue(item.Id.GetValueOrDefault(), out var prItemToUpdate))
                        {
                            // Existing record: update its properties.
                            prItemToUpdate.PurchaseRequestItemsId = item.Id.GetValueOrDefault();
                            prItemToUpdate.CatalogueId = item.CatalogueId;
                            prItemToUpdate.RequestedQuantity = item.RequestedQuantity;
                            prItemToUpdate.AmendedQuantity = item.AmendedQuantity;
                            prItemToUpdate.AmendedUnitPrice = item.AmendedUnitPrice;
                            prItemToUpdate.UnitPrice = item.UnitPrice;
                            prItemToUpdate.UpdatedDate = DateTime.Now;
                            prItemToUpdate.UpdatedByUserId = _userContext.UserId;
                            prItemToUpdate.Amount = item.Amount;
                            prItemToUpdate.IsActive = item.IsActive;
                            prItemToUpdate.AvailableAt = item.AvailableAt;
                            prItemToUpdate.IsFailed = item.IsFailed;
                            prItemToUpdate.ItemDescription = item.ItemDescription;
                            prItemToUpdate.ItemType = item.ItemTypeId;
                            prItemToUpdate.UnitOfMeasurement = item.UnitOfMeasurementId;
                        }
                        break;

                    case "new":
                        // New record: create a new instance and add it to the context.
                        var newPrItem = new PurchaseRequestItem
                        {
                            PurchaseRequestId = dto.Id.GetValueOrDefault(),
                            AmendedQuantity = item.AmendedQuantity,
                            AmendedUnitPrice = item.UnitPrice,
                            Amount = item.Amount,
                            AvailableAt = item.AvailableAt,
                            CatalogueId = item.CatalogueId,
                            CreatedByUserId = _userContext.UserId,
                            CreatedDate = DateTime.Now,
                            IsActive = true,
                            IsFailed = item.IsFailed,
                            ItemDescription = item.ItemDescription,
                            ItemType = item.ItemTypeId,
                            RequestedQuantity = item.RequestedQuantity,
                            RequestingOfficeId = item.RequestingOfficeId,
                            UnitOfMeasurement = item.UnitOfMeasurementId,
                            UnitPrice = item.UnitPrice
                        };
                        _context.PurchaseRequestItems.Add(newPrItem);
                        break;

                    case "deleted":
                        // Deleted record: mark it as inactive (soft delete).
                        if (existingPrItems.TryGetValue(item.Id.GetValueOrDefault(), out var prItemToDelete))
                        {
                            prItemToDelete.IsActive = false;
                            prItemToDelete.UpdatedDate = DateTime.Now;
                            prItemToDelete.UpdatedByUserId = _userContext.UserId;
                        }
                        break;

                    default:
                        // Items with no status or an unknown status are ignored.
                        break;
                }
            }
            #endregion

            #region Pr Item Details
            // Get a list of IDs from the DTO for records that are NOT new.
            var existingItemDetailIds = dto.PurchaseRequestItemDetails
                                 .Where(item => item.Id != 0)
                                 .Select(item => item.Id)
                                 .ToList();

            // Load all existing details in one go.
            var existingItemDetails = await _context.PurchaseRequestItemDetails
                .Where(c => existingItemDetailIds.Contains(c.PurchaseRequestItemDetailsId))
                .ToDictionaryAsync(c => c.PurchaseRequestItemDetailsId);

            foreach (var item in dto.PurchaseRequestItemDetails)
            {
                if (item.Id != 0 && existingItemDetails.TryGetValue(item.Id, out var itemDetail))
                {
                    // Existing record: update its properties.
                    itemDetail.PurchaseRequestItemsId = item.PurchaseRequestItemsId;
                    itemDetail.ItemSpecification = item.ItemSpecification;
                    itemDetail.UnitPrice = item.UnitPrice;
                    itemDetail.UnitOfMeasure = item.UnitOfMeasureId;
                    itemDetail.IsActive = item.IsActive;
                    itemDetail.UpdatedDate = item.UpdatedDate;
                    itemDetail.UpdatedByUserId = item.Updatedby;
                    itemDetail.RequestedQuantity = item.RequestedQuantity;
                    itemDetail.ItemType = item.ItemTypeId;
                }
                else
                {
                    // New record: create a new instance and add it to the context.
                    itemDetail = new PurchaseRequestItemDetail
                    {
                        PurchaseRequestItemsId = item.PurchaseRequestItemsId,
                        CreatedByUserId = _userContext.UserId,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        ItemSpecification = item.ItemSpecification,
                        ItemType = item.ItemTypeId,
                        RequestedQuantity = item.RequestedQuantity,
                        UnitOfMeasure = item.UnitOfMeasureId,
                        UnitPrice = item.UnitPrice
                    };
                    _context.PurchaseRequestItemDetails.Add(itemDetail);
                }
            }
            #endregion            

            #region Approval
            if (dto.TransactionStatus != null && dto.UserTransactionPermissions != null)
            {
                var trn = await AddTransactionStatus(28, dto.Id.GetValueOrDefault(), dto.UserTransactionPermissions, dto.TransactionStatus);

                _context.TransactionStatuses.Add(trn);

                #region Pr Status
                entity.Status = await GetTransactionStatus(dto.UserTransactionPermissions.WorkStepId, dto.TransactionStatus);

                if (dto.TransactionStatus.Disapproved)
                {
                    entity.IsSubmitted = false;
                }
                #endregion

                #region Generate PR Number if approval step is pr numbering
                if (trn.Status?.ToUpper() == "PR NUMBERING")
                {
                    // Generate PR Number
                    entity.Prnumber = GeneratePrNo(entity.BudgetYear);
                }
                #endregion
            }
            #endregion

            // update the PPMP model
            _dbSet.Update(entity);

            await _context.SaveChangesAsync();

            #region dissapproval
            // for disapproval and cancellation, set IsActive=false for transactionstatuses
            if (dto.TransactionStatus != null && dto.TransactionStatus.Disapproved)
            {
                await DisableTransactionStatuses(28, dto.Id.GetValueOrDefault());
            }
            #endregion
        }

        public async Task<List<PurchaseRequestItemDto>> GetPurchaseRequestItems(int purchaseRequestId)
        {
            var model = await _context.PurchaseRequestItems
                .Include(i => i.PurchaseRequestItemDetails)
                .Where(x => x.PurchaseRequestId == purchaseRequestId && x.IsActive)
                .ToListAsync();


            return model.Select(item => new PurchaseRequestItemDto
            {
                IsActive = item.IsActive,
                AmendedQuantity = item.AmendedQuantity,
                PurchaseRequestId = item.PurchaseRequestId,
                AmendedUnitPrice = item.AmendedUnitPrice,
                AvailableAt = item.AvailableAt,
                CatalogueId = item.CatalogueId,
                CreatedBy = item.CreatedByUserId,
                CreatedDate = item.CreatedDate,
                Id = item.PurchaseRequestItemsId,
                ItemDescription = item.ItemDescription,
                ItemTypeId = item.ItemType,
                RequestedQuantity = item.RequestedQuantity,
                RequestingOfficeId = item.RequestingOfficeId,
                UnitOfMeasurementId = item.UnitOfMeasurement,
                UnitPrice = item.UnitPrice.GetValueOrDefault(),
                Amount = item.Amount,
                IsFailed = item.IsFailed,
                UpdatedDate = item.UpdatedDate,
                Updatedby = item.UpdatedByUserId,
                PurchaseRequestItemDetails = item.PurchaseRequestItemDetails != null ? item.PurchaseRequestItemDetails.Select(detail => new PurchaseRequestItemDetailDto
                {
                    Id = detail.PurchaseRequestItemDetailsId,
                    PurchaseRequestItemsId = detail.PurchaseRequestItemsId,
                    ItemSpecification = detail.ItemSpecification,
                    RequestedQuantity = detail.RequestedQuantity,
                    UnitPrice = detail.UnitPrice.GetValueOrDefault(),
                    UnitOfMeasureId = detail.UnitOfMeasure,
                    ItemTypeId = detail.ItemType,
                    IsActive = detail.IsActive,
                    CreatedDate = detail.CreatedDate,
                    CreatedBy = detail.CreatedByUserId,
                    UpdatedDate = detail.UpdatedDate,
                    Updatedby = detail.UpdatedByUserId

                }) : []
            }).ToList();
        }

        protected override PRDto MapToDto(PurchaseRequest entity)
        {
            var dto = new PRDto
            {
                Id = entity.PurchaseRequestId,
                BudgetYear = entity.BudgetYear,
                Remarks = entity.Remarks,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                TotalAmount = entity.TotalAmount,
                IsActive = entity.IsActive,
                IsSubmitted = entity.IsSubmitted.GetValueOrDefault(),
                PrNumber = !string.IsNullOrWhiteSpace(entity.Prnumber) ? entity.Prnumber : entity.TempPrnumber,
                TempPrnumber = entity.TempPrnumber,
                RequestingOfficeId = entity.RequestingOffice,
                RequestingOffice = (_cachedItems.Departments.Result).FirstOrDefault(s => s.Id == entity.RequestingOffice)?.Name,
                SubmittedBy = entity.SubmittedBy,
                SubmittedDate = entity.SubmittedDate,
                DeletedBy = entity.DeletedByUserId,
                DeletedDate = entity.DeletedDate,
                IsDeleted = entity.DeletedDate.HasValue,
                Purpose = entity.Purpose,
                NumberOfItems = entity.PurchaseRequestItems.Where(x => x.IsActive).Count(),
                PurchasingType = entity.PurchasingType,
                PrClassification = entity.Prclassification,
                ConsolidatedBy = entity.ConsolidatedBy,
                Prkind = entity.Prkind,
                ContractId = entity.ContractId,
                DatePrepared = entity.DatePrepared,
                UpdatedDate = entity.UpdatedDate,
                Updatedby = entity.UpdatedByUserId,
                FirstQuarter = entity.FirstQuarter,
                SecondQuarter = entity.SecondQuarter,
                ThirdQuarter = entity.ThirdQuarter,
                FourthQuarter = entity.FourthQuarter,
                HasEquipments = entity.HasEquipments,
                HasSupplies = entity.HasSupplies,
                IsConsolidated = entity.IsConsolidated,
                IsForDelivery = entity.IsForDelivery,
                IsContract = entity.IsContract,
                IsPartialEquipment = entity.IsPartialEquipment,
                IsPartialSupply = entity.IsPartialSupply,
                IsRepeatOrder = entity.IsRepeatOrder,
                ModeOfProcurement = entity.ModeOfProcurement,
                SupplierId = entity.Supplier,
                PpmpId = entity.PpmpId,
                PpmpNumber = entity.PpmpNumber,
                IsForRsqrfq = entity.IsForRsqrfq,
                IsItemized = entity.IsItemized,
                IsPrSo = entity.IsPrSo,
                OtherRemarks = entity.OtherRemarks,
                IsCreatedBySupplyOfficer = entity.IsCreatedBySupplyOfficer,
                WithApr = entity.WithApr,
                WithCanvassing = entity.WithCanvassing,
                WithoutPo = entity.WithoutPo,



                //RequestingOffice = entity.Req != null ? new DepartmentDto
                //{
                //    Name = entity.RequestingOffice.Name,
                //    Code = entity.RequestingOffice.Code,
                //    Description = entity.RequestingOffice.Description,
                //} : null,


            };
            return dto;
        }

        protected override PurchaseRequest MapToEntity(PRDto dto)
        {
            var entity = new PurchaseRequest
            {
                PurchaseRequestId = dto.Id.GetValueOrDefault(),
                Prnumber = dto.PrNumber,
                BudgetYear = dto.BudgetYear,
                Remarks = dto.Remarks,
                Status = dto.IsSubmitted ? "submitted" : dto.Status,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy,
                IsActive = dto.IsActive,
                IsSubmitted = dto.IsSubmitted,
                SubmittedBy = dto.IsSubmitted ? _userContext.UserId : null,
                SubmittedDate = dto.IsSubmitted ? DateTime.Now : null,
                DeletedByUserId = dto.DeletedBy,
                DeletedDate = dto.DeletedDate,
                UpdatedDate = dto.UpdatedDate,
                UpdatedByUserId = dto.Updatedby,
                TotalAmount = dto.TotalAmount,
                RequestingOffice = dto.RequestingOfficeId,
                TempPrnumber = dto.TempPrnumber,
                Purpose = dto.Purpose,
                PurchasingType = dto.PurchasingType,
                Prclassification = dto.PrClassification,
                ConsolidatedBy = dto.ConsolidatedBy,
                Prkind = dto.Prkind,
                ContractId = dto.ContractId,
                DatePrepared = dto.DatePrepared,
                //UpdatedDate = dto.UpdatedDate,
                //Updatedby = dto.UpdatedByUserId,
                FirstQuarter = dto.FirstQuarter,
                SecondQuarter = dto.SecondQuarter,
                ThirdQuarter = dto.ThirdQuarter,
                FourthQuarter = dto.FourthQuarter,
                HasEquipments = dto.HasEquipments,
                HasSupplies = dto.HasSupplies,
                IsConsolidated = dto.IsConsolidated,
                IsForDelivery = dto.IsForDelivery,
                IsContract = dto.IsContract,
                IsPartialEquipment = dto.IsPartialEquipment,
                IsPartialSupply = dto.IsPartialSupply,
                IsRepeatOrder = dto.IsRepeatOrder,
                ModeOfProcurement = dto.ModeOfProcurement,
                Supplier = dto.SupplierId,
                PpmpId = dto.PpmpId,
                PpmpNumber = dto.PpmpNumber,
                IsForRsqrfq = dto.IsForRsqrfq,
                IsItemized = dto.IsItemized,
                IsPrSo = dto.IsPrSo,
                OtherRemarks = dto.OtherRemarks,
                IsCreatedBySupplyOfficer = dto.IsCreatedBySupplyOfficer,
                WithApr = dto.WithApr,
                WithCanvassing = dto.WithCanvassing,
                WithoutPo = dto.WithoutPo,

                // Populate PrItems for Create ONLY(has PurchaseRequestId)
                PurchaseRequestItems = dto.Id.GetValueOrDefault() == 0 ? dto.PurchaseRequestItems.Select(prItem => new PurchaseRequestItem
                {
                    PurchaseRequestItemsId = prItem.Id.GetValueOrDefault(),
                    PurchaseRequestId = prItem.PurchaseRequestId,
                    CatalogueId = prItem.CatalogueId,
                    RequestedQuantity = prItem.RequestedQuantity,
                    AmendedQuantity = prItem.AmendedQuantity,
                    AmendedUnitPrice = prItem.AmendedUnitPrice,
                    UnitPrice = prItem.UnitPrice,
                    Amount = prItem.Amount,
                    IsActive = prItem.IsActive,
                    CreatedDate = DateTime.Now,
                    CreatedByUserId = _userContext.UserId,
                    UpdatedDate = prItem.UpdatedDate,
                    UpdatedByUserId = prItem.Updatedby,
                    AvailableAt = prItem.AvailableAt,
                    IsFailed = prItem.IsFailed,
                    ItemDescription = prItem.ItemDescription,
                    ItemType = prItem.ItemTypeId,
                    UnitOfMeasurement = prItem.UnitOfMeasurementId,

                    PurchaseRequestItemDetails = prItem.Id == 0 ? dto.PurchaseRequestItemDetails.Where(d => d.PurchaseRequestItemsId == prItem.Id).Select(detail => new PurchaseRequestItemDetail
                    {
                        PurchaseRequestItemsId = prItem.Id.GetValueOrDefault(),
                        ItemSpecification = detail.ItemSpecification,
                        RequestedQuantity = detail.RequestedQuantity,
                        UnitPrice = detail.UnitPrice,
                        UnitOfMeasure = detail.UnitOfMeasureId,
                        ItemType = detail.ItemTypeId,
                        CreatedDate = DateTime.Now,
                        CreatedByUserId = _userContext.UserId,
                        IsActive = true
                    }).ToList() : []

                }).ToList() : []
            };

            return entity;
        }

        protected PurchaseRequest MapToEntity(PurchaseRequest entity, PRDto dto)
        {
            entity.PurchaseRequestId = dto.Id.GetValueOrDefault();
            entity.Prnumber = dto.PrNumber;
            entity.BudgetYear = dto.BudgetYear;
            entity.Remarks = dto.Remarks;
            entity.Status = dto.IsSubmitted ? "submitted" : dto.Status;
            entity.CreatedDate = dto.CreatedDate;
            entity.CreatedByUserId = dto.CreatedBy;
            entity.IsActive = dto.IsActive;
            entity.IsSubmitted = dto.IsSubmitted;
            entity.SubmittedBy = dto.IsSubmitted ? _userContext.UserId : null;
            entity.SubmittedDate = dto.IsSubmitted ? DateTime.Now : null;
            entity.DeletedByUserId = dto.DeletedBy;
            entity.DeletedDate = dto.DeletedDate;
            entity.UpdatedDate = dto.UpdatedDate;
            entity.UpdatedByUserId = dto.Updatedby;
            entity.TotalAmount = dto.TotalAmount;
            entity.RequestingOffice = dto.RequestingOfficeId;
            //entity.TempPrnumber = dto.TempPrnumber;
            entity.Purpose = dto.Purpose;
            entity.PurchasingType = dto.PurchasingType;
            entity.Prclassification = dto.PrClassification;
            entity.ConsolidatedBy = dto.ConsolidatedBy;
            entity.Prkind = dto.Prkind;
            entity.ContractId = dto.ContractId;
            entity.DatePrepared = dto.DatePrepared;
            //entity.//UpdatedDate = dto.UpdatedDate;
            //entity.//Updatedby = dto.UpdatedByUserId;
            entity.FirstQuarter = dto.FirstQuarter;
            entity.SecondQuarter = dto.SecondQuarter;
            entity.ThirdQuarter = dto.ThirdQuarter;
            entity.FourthQuarter = dto.FourthQuarter;
            entity.HasEquipments = dto.HasEquipments;
            entity.HasSupplies = dto.HasSupplies;
            entity.IsConsolidated = dto.IsConsolidated;
            entity.IsForDelivery = dto.IsForDelivery;
            entity.IsContract = dto.IsContract;
            entity.IsPartialEquipment = dto.IsPartialEquipment;
            entity.IsPartialSupply = dto.IsPartialSupply;
            entity.IsRepeatOrder = dto.IsRepeatOrder;
            entity.ModeOfProcurement = dto.ModeOfProcurement;
            entity.Supplier = dto.SupplierId;
            entity.PpmpId = dto.PpmpId;
            entity.PpmpNumber = dto.PpmpNumber;
            entity.IsForRsqrfq = dto.IsForRsqrfq;
            entity.IsItemized = dto.IsItemized;
            entity.IsPrSo = dto.IsPrSo;
            entity.OtherRemarks = dto.OtherRemarks;
            entity.IsCreatedBySupplyOfficer = dto.IsCreatedBySupplyOfficer;
            entity.WithApr = dto.WithApr;
            entity.WithCanvassing = dto.WithCanvassing;
            entity.WithoutPo = dto.WithoutPo;

            //// Populate PrItems for Create ONLY(has PurchaseRequestId)
            //entity.PurchaseRequestItems = dto.Id.GetValueOrDefault() == 0 ? dto.PurchaseRequestItems.Select(prItem => new PurchaseRequestItem
            //{
            //    PurchaseRequestItemsId = prItem.Id.GetValueOrDefault(),
            //    PurchaseRequestId = prItem.PurchaseRequestId,
            //    CatalogueId = prItem.CatalogueId,
            //    RequestedQuantity = prItem.RequestedQuantity,
            //    AmendedQuantity = prItem.AmendedQuantity,
            //    AmendedUnitPrice = prItem.AmendedUnitPrice,
            //    UnitPrice = prItem.UnitPrice,
            //    Amount = prItem.Amount,
            //    IsActive = prItem.IsActive,
            //    CreatedDate = DateTime.Now,
            //    CreatedByUserId = _userContext.UserId,
            //    UpdatedDate = prItem.UpdatedDate,
            //    UpdatedByUserId = prItem.Updatedby,
            //    AvailableAt = prItem.AvailableAt,
            //    IsFailed = prItem.IsFailed,
            //    ItemDescription = prItem.ItemDescription,
            //    ItemType = prItem.ItemTypeId,
            //    UnitOfMeasurement = prItem.UnitOfMeasurementId,

            //    PurchaseRequestItemDetails = prItem.Id == 0 ? dto.PurchaseRequestItemDetails.Where(d => d.PurchaseRequestItemsId == prItem.Id).Select(detail => new PurchaseRequestItemDetail
            //    {
            //        PurchaseRequestItemsId = prItem.Id.GetValueOrDefault(),
            //        ItemSpecification = detail.ItemSpecification,
            //        RequestedQuantity = detail.RequestedQuantity,
            //        UnitPrice = detail.UnitPrice,
            //        UnitOfMeasure = detail.UnitOfMeasureId,
            //        ItemType = detail.ItemTypeId,
            //        CreatedDate = DateTime.Now,
            //        CreatedByUserId = _userContext.UserId,
            //        IsActive = true
            //    }).ToList() : []

            //}).ToList() : [];

            return entity;
        }

        #endregion

        #region private methods

        private string GeneratePrNo(short budgetYear)
        {
            // Retrieve the last PPMP record for the given budget year
            var lastRecord = _context.PurchaseRequests
                .Where(pr => pr.BudgetYear == budgetYear
                    && (pr.Prnumber != null && pr.Prnumber != "")// only get those with real PrNumber
                )
                .OrderByDescending(ppmp => ppmp.PurchaseRequestId)
                .FirstOrDefault();

            string referenceNo = lastRecord?.Prnumber ?? $"PR-{budgetYear}-{DateTime.Now.Month}-000";

            // Split reference number
            var splitReferenceNo = referenceNo.Split('-');

            // Ensure the year is up-to-date
            splitReferenceNo[1] = budgetYear.ToString(CultureInfo.InvariantCulture);

            // Increment the series number or reset if it's a new year
            int seriesNumber = (splitReferenceNo[1] == budgetYear.ToString(CultureInfo.InvariantCulture))
                ? int.Parse(splitReferenceNo[3]) + 1
                : 1;

            splitReferenceNo[3] = seriesNumber.ToString("D3"); // Ensures 3-digit formatting

            return string.Join("-", splitReferenceNo);
        }

        private string GenerateTempPrNo(short budgetYear)
        {
            // Retrieve the last PPMP record for the given budget year
            var lastRecord = _context.PurchaseRequests
                .Where(ppmp => ppmp.BudgetYear == budgetYear)
                .OrderByDescending(ppmp => ppmp.PurchaseRequestId)
                .FirstOrDefault();

            string referenceNo = lastRecord?.TempPrnumber ?? $"TEMP-{budgetYear}-000";

            // Split reference number
            var splitReferenceNo = referenceNo.Split('-');

            // Ensure the year is up-to-date
            splitReferenceNo[1] = budgetYear.ToString(CultureInfo.InvariantCulture);

            // Increment the series number or reset if it's a new year
            int seriesNumber = (splitReferenceNo[1] == budgetYear.ToString(CultureInfo.InvariantCulture))
                ? int.Parse(splitReferenceNo[2]) + 1
                : 1;

            splitReferenceNo[2] = seriesNumber.ToString("D3"); // Ensures 4-digit formatting

            return string.Join("-", splitReferenceNo);
        }

        public Task<List<VPrPpmpPsdbmcatalogue>> GetRemainingPpmpCatalogue(short budgetYear, int requestingOffice)
        {
            return _context.VPrPpmpPsdbmcatalogues
                .Where(x => x.BudgetYear == budgetYear && x.RequestingOfficeId == requestingOffice && x.IsActive)
                .ToListAsync();
        }

        #endregion
    }
}
