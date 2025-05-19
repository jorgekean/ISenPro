using EF.Models;
using EF.Models.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Service.Transaction
{
    public class AppService : BaseService<App, APPDto>, IAppService
    {
        private readonly CachedItems _cachedItems;

        public AppService(ISenProContext context, IUserContext userContext,
            CachedItems cachedItems) : base(context, userContext)
        {
            _cachedItems = cachedItems;
        }

        #region CRUD
        //protected override IQueryable<T> ApplySearchFilter<T>(IQueryable<T> query, string searchQuery)
        //{
        //    // If the type is MyEntity, cast the query and apply filtering.
        //    if (typeof(T) == typeof(VAppindex))
        //    {
        //        var typedQuery = query as IQueryable<VAppindex>;
        //        if (!string.IsNullOrWhiteSpace(searchQuery))
        //        {
        //            typedQuery = typedQuery.Where(p => new[] { p.BudgetYear.ToString(), p.Remarks, p.Status, p.Appno, p.OfficeName, p.PreparedBy }
        //                     .Any(value => value != null && value.Contains(searchQuery)));
        //        }

        //        // Cast back to IQueryable<T> and return
        //        return typedQuery as IQueryable<T>;
        //    }

        //    // Otherwise, for any other type, you can either return the unfiltered query or add your own logic.
        //    return query;
        //}

        protected override IQueryable<App> ApplySearchFilter(IQueryable<App> query, string searchQuery)
        {
            return query.Where(p => new[] { p.Appno, p.BudgetYear.ToString() }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        //public override IQueryable<T> ApplyFilterCriteria<T>(IQueryable<T> query)
        //{
        //    if (typeof(T) == typeof(App))
        //    {
        //        var userId = _userContext.UserId;
        //        var isAdmin = _userContext.IsAdmin;
        //        var parentModule = 1;

        //        var typedQuery = (IQueryable<App>)query;

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

        public override async Task<APPDto> GetByIdAsync(int id)
        {
            var model = await base.GetByIdAsync(id);

            var userPermission = await _context.GetUserTransactionPermissionsAsync(id, _userContext.UserId, 25);

            model.UserTransactionPermissions = userPermission;
            model.CanApprove = model.IsSubmitted && userPermission != null && userPermission.CanApprove;

            //var psdbms = _context.VAppPsdbmcatalogues
            //             .Where(x => x.Appid == id && x.IsActive)
            //             .ToList();

            //var supps = _context.VAppSupplementaryCatalogues
            //             .Where(x => x.Appid == id && x.IsActive)
            //             .ToList();

            //var projects = _context.Appprojects.Include(i => i.AccountCode)
            //             .Where(x => x.Appid == id && x.IsActive)
            //             .ToList();           

            return model;
        }

        public override async Task<object> AddAsync(APPDto dto)
        {
            dto.AppNo = GenerateAppNo(dto.BudgetYear);

            return await base.AddAsync(dto);
        }

        // Expect to received the updated/added appcatalogues and appsupplementaries ONLY
        public override async Task UpdateAsync(APPDto dto)
        {
            var entity = MapToEntity(dto);

            #region for review
            //#region APP Catalogues
            //// Get a list of IDs from the DTO for records that are NOT new.
            //var existingIds = dto.Appcatalogues
            //                     .Where(item => item.Id != 0)
            //                     .Select(item => item.Id)
            //                     .ToList();

            //// Load all existing Appcatalogues in one go.
            //var existingCatalogues = await _context.Appcatalogues
            //    .Where(c => existingIds.Contains(c.AppcatalogueId))
            //    .ToDictionaryAsync(c => c.AppcatalogueId);

            //foreach (var item in dto.Appcatalogues)
            //{
            //    if (item.Id != 0 && existingCatalogues.TryGetValue(item.Id, out var catalogue))
            //    {
            //        // Existing record: update its properties.
            //        catalogue.Appid = dto.Id;
            //        catalogue.CatalogueId = item.CatalogueId;
            //        catalogue.FirstQuarter = item.FirstQuarter;
            //        catalogue.SecondQuarter = item.SecondQuarter;
            //        catalogue.ThirdQuarter = item.ThirdQuarter;
            //        catalogue.FourthQuarter = item.FourthQuarter;
            //        catalogue.UnitPrice = item.UnitPrice;
            //        catalogue.Amount = item.Amount;
            //        catalogue.IsActive = item.IsActive;
            //        catalogue.CreatedDate = item.CreatedDate;
            //        catalogue.CreatedByUserId = item.CreatedBy;
            //        catalogue.UpdatedDate = item.UpdatedDate;
            //        catalogue.UpdatedByUserId = item.Updatedby;
            //        catalogue.DeletedDate = item.DeletedDate;
            //        catalogue.DeletedByUserId = item.DeletedBy;
            //        catalogue.Description = item.Description;
            //        catalogue.Remarks = item.Remarks;
            //    }
            //    else
            //    {
            //        // New record: create a new instance and add it to the context.
            //        catalogue = new Appcatalogue
            //        {
            //            Appid = dto.Id,
            //            CatalogueId = item.CatalogueId,
            //            FirstQuarter = item.FirstQuarter,
            //            SecondQuarter = item.SecondQuarter,
            //            ThirdQuarter = item.ThirdQuarter,
            //            FourthQuarter = item.FourthQuarter,
            //            UnitPrice = item.UnitPrice,
            //            Amount = item.Amount,
            //            IsActive = true,
            //            CreatedDate = DateTime.Now,
            //            CreatedByUserId = 1, // TO DO: Use the current user's id
            //            Description = item.Description,
            //            Remarks = item.Remarks
            //        };
            //        _context.Appcatalogues.Add(catalogue);
            //    }
            //}
            //#endregion

            //#region APP Supplementaries
            //// Get a list of IDs from the DTO for records that are NOT new.
            //var existingSuppIds = dto.Appsupplementaries
            //                     .Where(item => item.Id != 0)
            //                     .Select(item => item.Id)
            //                     .ToList();

            //// Load all existing AppSupplementaries in one go.
            //var existingSupps = await _context.Appsupplementaries
            //    .Where(c => existingSuppIds.Contains(c.AppsupplementaryId))
            //    .ToDictionaryAsync(c => c.AppsupplementaryId);

            //foreach (var item in dto.Appsupplementaries)
            //{
            //    if (item.Id != 0 && existingSupps.TryGetValue(item.Id, out var supplementary))
            //    {
            //        // Existing record: update its properties.
            //        supplementary.Appid = dto.Id;
            //        supplementary.SupplementaryId = item.SupplementaryId;
            //        supplementary.FirstQuarter = item.FirstQuarter;
            //        supplementary.SecondQuarter = item.SecondQuarter;
            //        supplementary.ThirdQuarter = item.ThirdQuarter;
            //        supplementary.FourthQuarter = item.FourthQuarter;
            //        supplementary.UnitPrice = item.UnitPrice;
            //        supplementary.Amount = item.Amount;
            //        supplementary.IsActive = item.IsActive;
            //        supplementary.CreatedDate = item.CreatedDate;
            //        supplementary.CreatedByUserId = item.CreatedBy;
            //        supplementary.UpdatedDate = item.UpdatedDate;
            //        supplementary.UpdatedByUserId = item.Updatedby;
            //        supplementary.DeletedDate = item.DeletedDate;
            //        supplementary.DeletedByUserId = item.DeletedBy;
            //        supplementary.Description = item.Description;
            //        supplementary.Remarks = item.Remarks;
            //    }
            //    else
            //    {
            //        // New record: create a new instance and add it to the context.
            //        supplementary = new Appsupplementary
            //        {
            //            Appid = dto.Id,
            //            SupplementaryId = item.SupplementaryId,
            //            FirstQuarter = item.FirstQuarter,
            //            SecondQuarter = item.SecondQuarter,
            //            ThirdQuarter = item.ThirdQuarter,
            //            FourthQuarter = item.FourthQuarter,
            //            UnitPrice = item.UnitPrice,
            //            Amount = item.Amount,
            //            IsActive = true,
            //            CreatedDate = DateTime.Now,
            //            CreatedByUserId = 1, // TO DO: Use the current user's id
            //            Description = item.Description,
            //            Remarks = item.Remarks
            //        };
            //        _context.Appsupplementaries.Add(supplementary);
            //    }
            //}
            //#endregion

            //#region APP Projects
            //// Get a list of IDs from the DTO for records that are NOT new.
            //var existingProjectIds = dto.AppProjects
            //                     .Where(item => item.Id != 0)
            //                     .Select(item => item.Id)
            //                     .ToList();

            //// Load all existing AppSupplementaries in one go.
            //var existingProjs = await _context.Appprojects
            //    .Where(c => existingProjectIds.Contains(c.AppprojectId))
            //    .ToDictionaryAsync(c => c.AppprojectId);

            //foreach (var item in dto.AppProjects)
            //{
            //    if (item.Id != 0 && existingProjs.TryGetValue(item.Id, out var project))
            //    {
            //        // Existing record: update its properties.
            //        project.AppprojectId = item.Id;
            //        project.Appid = item.Appid;
            //        project.ProjectName = item.ProjectName;
            //        project.Description = item.Description;
            //        project.Quarter = item.Quarter;
            //        project.Cost = item.Cost;
            //        project.ProjectStatus = item.ProjectStatus;
            //        project.IsActive = item.IsActive;
            //        project.CreatedByUserId = item.CreatedBy;
            //        project.CreatedDate = item.CreatedDate;
            //        project.AccountCodeId = item.AccountCode?.Id;
            //    }
            //    else
            //    {
            //        // New record: create a new instance and add it to the context.
            //        project = new Appproject
            //        {
            //            AppprojectId = item.Id,
            //            Appid = item.Appid,
            //            ProjectName = item.ProjectName,
            //            Description = item.Description,
            //            Quarter = item.Quarter,
            //            Cost = item.Cost,
            //            ProjectStatus = item.ProjectStatus,
            //            IsActive = item.IsActive,
            //            CreatedByUserId = item.CreatedBy,
            //            CreatedDate = item.CreatedDate,
            //            AccountCodeId = item.AccountCode?.Id
            //        };
            //        _context.Appprojects.Add(project);
            //    }
            //}
            //#endregion

            //#region Approval
            //if (dto.TransactionStatus != null && dto.UserTransactionPermissions != null)//  && (dto.TransactionStatus.Approved || dto.TransactionStatus.Disapproved)
            //{
            //    var trn = await AddTransactionStatus(25, dto.Id, dto.UserTransactionPermissions, dto.TransactionStatus);

            //    _context.TransactionStatuses.Add(trn);

            //    #region App Status
            //   entity.Status = await GetTransactionStatus(dto.UserTransactionPermissions.WorkStepId, dto.TransactionStatus);

            //    if (dto.TransactionStatus.Disapproved)
            //    {
            //        entity.IsSubmitted = false;
            //    }

            //    #endregion
            //}
            //#endregion
            #endregion

            // update the APP model
            _dbSet.Update(entity);

            await _context.SaveChangesAsync();

            //#region dissapproval
            //// for disapproval and cancellation, set IsActive=false or transactionstatuses
            //if (dto.TransactionStatus != null && dto.TransactionStatus.Disapproved)
            //{
            //   await DisableTransactionStatuses(25, dto.Id);
            //}
            //#endregion
        }

        protected override APPDto MapToDto(App entity)
        {
            var dto = new APPDto
            {
                Id = entity.Appid,
                BudgetYear = entity.BudgetYear,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                AdditionalInflationValue = entity.AdditionalInflationValue,
                AdditionalTenPercent = entity.AdditionalTenPercent,
                TotalAmount = entity.TotalAmount,
                GrandTotalAmount = entity.GrandTotal,
                TotalCatalogueAmount = 0,
                TotalProjectAmount = 0,
                TotalSupplementaryAmount = 0,

                IsActive = entity.IsActive,
                IsSubmitted = entity.IsSubmitted,
                AppNo = entity.Appno,
                SubmittedBy = entity.SubmittedByUserId,
                SubmittedDate = entity.SubmittedDate,
                DeletedBy = entity.DeletedByUserId,
                DeletedDate = entity.DeletedDate,
                IsDeleted = entity.DeletedDate.HasValue,         
            };
            return dto;
        }

        protected override App MapToEntity(APPDto dto)
        {
            var entity = new App
            {
                Appid = dto.Id,
                Appno = dto.AppNo,
                BudgetYear = dto.BudgetYear,
                Status = dto.IsSubmitted ? "submitted" : dto.Status,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy,
                IsActive = dto.IsActive,
                IsSubmitted = dto.IsSubmitted,
                SubmittedByUserId = dto.IsSubmitted ? _userContext.UserId : null,
                SubmittedDate = dto.IsSubmitted ? DateTime.Now : null,
                DeletedByUserId = dto.DeletedBy,
                DeletedDate = dto.DeletedDate,
                UpdatedDate = dto.UpdatedDate,
                UpdatedByUserId = dto.Updatedby,
                AdditionalInflationValue = dto.AdditionalInflationValue,
                AdditionalTenPercent = dto.AdditionalTenPercent,
                TotalAmount = dto.TotalAmount,
                GrandTotal = dto.GrandTotalAmount
            };

            return entity;
        }

        #endregion

        #region Reports
        //  Implement the APPGenerateReport method here
        public async Task GenerateReport(int appId)
        {
            string templatePath = "C:\\Users\\Jorge\\Documents\\Freelance\\PPMS\\Report Templates\\AppReportTemplateMergeFieldWithChart.docx";
            string outputPath = $"C:\\Users\\Jorge\\Documents\\Freelance\\PPMS\\Report Output\\APP_Output_{DateTime.Now.ToString("MMddyyyyHHmmss")}.docx";

            //// Load the APP record
            //var app = await _context.Apps
            //    .Include(p => p.RequestingOffice)
            //    .FirstOrDefaultAsync(p => p.Appid == appId);

            //// Load the APP Catalogues
            //var appCatalogues = await _context.Appcatalogues
            //    .Include(c => c.Catalogue)
            //    .Where(c => c.Appid == appId)
            //    .ToListAsync();

            //// Load the APP Supplementaries
            //var appSupplementaries = await _context.Appsupplementaries
            //    .Include(s => s.Supplementary)
            //    .Where(s => s.Appid == appId)
            //    .ToListAsync();

            //// Load the APP Projects
            //var appProjects = await _context.Appprojects
            //    .Include(p => p.AccountCode)
            //    .Where(p => p.Appid == appId)
            //    .ToListAsync();

            //// Prepare the placeholders
            //var placeholders = new Dictionary<string, string>
            //{
            //    { BaseGenerateReport.MERGEFIELD_BUDGETYEAR, app.BudgetYear.ToString() },
            //    { BaseGenerateReport.MERGEFIELD_DEPARTMENTNAME, app.RequestingOffice.Name }
            //};

            //var generator = new APPGenerateReport();
            //generator.Items = appCatalogues.Select(c => new APPItem
            //{
            //    Description = c.Catalogue.Description,
            //    Qtr1 = c.FirstQuarter,
            //    Qtr2 = c.SecondQuarter,
            //    Qtr3 = c.ThirdQuarter,
            //    Qtr4 = c.FourthQuarter,
            //    Amount = c.Amount,
            //    Remarks = c.Remarks,
            //    UnitPrice = c.UnitPrice,
            //    //UOM = c.
            //}).ToList();
            //generator.GenerateReport(templatePath, outputPath, placeholders);

            // Replace the table placeholders
            //ReplaceTablePlaceholders(outputPath, appCatalogues, appSupplementaries, appProjects);
        }
        #endregion

        public IList<int> GetBudgetYears()
        {
            // to do

            return [2025, 2026, 2027];
        }

        #region private methods

        private string GenerateAppNo(short budgetYear)
        {
            // Retrieve the last APP record for the given budget year
            var appLastRecord = _context.Apps
                .Where(app => app.BudgetYear == budgetYear)
                .OrderByDescending(app => app.Appid)
                .FirstOrDefault();

            string referenceNo = appLastRecord?.Appno ?? $"APP-{budgetYear}";

            // Split reference number
            var splitReferenceNo = referenceNo.Split('-');

            // Ensure the year is up-to-date
            splitReferenceNo[1] = budgetYear.ToString(CultureInfo.InvariantCulture);

            //// Increment the series number or reset if it's a new year
            //int seriesNumber = (splitReferenceNo[1] == budgetYear.ToString(CultureInfo.InvariantCulture))
            //    ? int.Parse(splitReferenceNo[2]) + 1
            //    : 1;

            //splitReferenceNo[2] = seriesNumber.ToString("D4"); // Ensures 4-digit formatting

            return string.Join("-", splitReferenceNo);
        }
        #endregion
    }
}
