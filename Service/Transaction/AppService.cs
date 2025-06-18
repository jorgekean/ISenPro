using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using EF.Models;
using EF.Models.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using Service.Cache;
using Service.Constants;
using Service.Dto.SystemSetup;
using Service.Dto.Transaction;
using Service.Dto.UserManagement;
using Service.Reports;
using Service.Reports.Transactions;
using Service.Service;
using Service.SystemSetup.Interface;
using Service.Transaction.Interface;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
            return query.Where(p => new[] { p.Appno, p.BudgetYear.ToString(), p.Status }
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
            dto.AppNo = GenerateAppNo(dto.BudgetYear.GetValueOrDefault());

            // total amounts
            var totalAmount = 0m;
            var psdbmItems = await ViewConsolidated(dto.BudgetYear.GetValueOrDefault());
            var supplementaryItems = await ViewConsolidatedSuppItems(dto.BudgetYear.GetValueOrDefault());
            var projectItems = await ViewConsolidatedProjectItems(dto.BudgetYear.GetValueOrDefault());

            totalAmount += psdbmItems.Sum(i => i.Amount) +
                          supplementaryItems.Sum(i => i.Amount) +
                          projectItems.Sum(i => i.Cost);

            dto.TotalAmount = totalAmount;
            dto.AdditionalInflationValue = 10; // static to 10 for now
            dto.GrandTotalAmount = totalAmount + (totalAmount * (dto.AdditionalInflationValue / 100));

            return await base.AddAsync(dto);
        }

        // Expect to received the updated/added appcatalogues and appsupplementaries ONLY
        public override async Task UpdateAsync(APPDto dto)
        {

            // AppDetails - Skip if PpmpId exists in the database
            var existingAppDetails = await _context.Appdetails.Where(x => x.AppId == dto.Id && x.IsActive).ToListAsync();
            var newAppDetails = dto.AppDetails?
                .Where(ad => ad.PpmpId.HasValue && !existingAppDetails.Any(ed => ed.PpmpId == ad.PpmpId)).ToList();
            dto.AppDetails = newAppDetails;

            var entity = MapToEntity(dto);

            #region Submit means approved for APP
            if (dto.IsSubmitted)
            {
                var trn = new TransactionStatus
                {
                    //RequiredApprover = dto.TransactionStatus.RequiredApprover,
                    CreatedDate = DateTime.Now,
                    IsCurrent = true,// TBD - NO NEED FOR THIS
                    Count = 1,
                    IsDone = true,
                    PageId = 26, // ModuleId
                    ProcessByUserId = _userContext.UserId,
                    Remarks = "Submitted",
                    Status = "Approved",
                    TransactionId = dto.Id.GetValueOrDefault(),
                    WorkstepId = 0,
                    Action = "approved",
                    IsActive = true,
                };

                _context.TransactionStatuses.Add(trn);

                entity.Status = "approved";
            }
            #endregion

            // update the APP model
            _dbSet.Update(entity);

            await _context.SaveChangesAsync();
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
                Appid = dto.Id.GetValueOrDefault(),
                Appno = dto.AppNo,
                BudgetYear = dto.BudgetYear.GetValueOrDefault(),
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
                AdditionalInflationValue = 10,// static to 10 for now
                AdditionalTenPercent = dto.AdditionalTenPercent,// to be removed?
                TotalAmount = dto.TotalAmount,
                GrandTotal = dto.GrandTotalAmount
            };

            entity.Appdetails = dto.AppDetails?
                .Select(ad => new Appdetail
                {
                    AppId = entity.Appid,
                    PpmpId = ad.PpmpId.GetValueOrDefault(),
                    AppdetailId = ad.Id.GetValueOrDefault(),
                    CreatedDate = DateTime.Now,
                    CreatedByUserId = _userContext.UserId,
                    IsActive = true,
                }).ToList() ?? new List<Appdetail>();

            return entity;
        }

        #endregion

        public async Task<List<APPDetailsPPMPDto>> GetOfficesNoPPMPs(short budgetYear)
        {
            // SELECT d.Name FROM dbo.Departments d
            // LEFT JOIN dbo.PPMPs p ON p.RequestingOfficeId = d.DepartmentId AND YEAR(p.BudgetYear)= @budgetyear
            //WHERE d.IsActive = 1 AND p.PPMPId IS NULL
            var result = await _context.UmDepartments
                .Where(d => d.IsActive && !_context.Ppmps.Any(p => p.RequestingOfficeId == d.DepartmentId && p.BudgetYear == budgetYear))
                .Select(d => new APPDetailsPPMPDto
                {
                    BudgetYear = budgetYear,
                    RequestingOffice = d.Name ?? "",
                    OfficeCode = d.Code ?? ""
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<APPDetailsPPMPDto>> GetOfficesWithApprovedPPMPs(short budgetYear)
        {

            //SELECT* FROM dbo.PPMPs a
            //JOIN dbo.UM_Department d ON d.DepartmentId = a.RequestingOfficeId
            //WHERE a.IsActive = 1 AND a.Status = 'approved'
            //AND a.BudgetYear = @budgetyear
            var result = await _context.Ppmps.Include(d => d.RequestingOffice)
                .Where(x => x.Status == TransactionStatusConst.Approved && x.IsActive && x.BudgetYear == budgetYear)
                .Select(x => new APPDetailsPPMPDto
                {
                    PpmpId = x.Ppmpid,
                    BudgetYear = x.BudgetYear,
                    PpmpNo = x.Ppmpno,
                    RequestingOffice = x.RequestingOffice.Name ?? "",
                    OfficeCode = x.RequestingOffice.Code ?? "",
                    DateSubmitted = x.SubmittedDate.GetValueOrDefault().ToString("MM/dd/yyyy")
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<APPDetailsPPMPDto>> GetOfficesWithSavedPPMPs(short budgetYear)
        {
            var result = await _context.Ppmps.Include(d => d.RequestingOffice)
                .Where(x => x.Status != TransactionStatusConst.Approved && x.IsActive && x.BudgetYear == budgetYear)
                .Select(x => new APPDetailsPPMPDto
                {
                    BudgetYear = x.BudgetYear,
                    PpmpNo = x.Ppmpno,
                    RequestingOffice = x.RequestingOffice.Name ?? "",
                    OfficeCode = x.RequestingOffice.Code ?? "",
                    DateSubmitted = x.SubmittedDate.GetValueOrDefault().ToString("MM/dd/yyyy")
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<APPCatalogueDto>> ViewConsolidated(short budgetYear)
        {
            var approvedPpmps = (await GetOfficesWithApprovedPPMPs(budgetYear)).Select(s => s.PpmpId);

            //SELECT*
            //FROM dbo.PPMPs a
            //JOIN dbo.PPMPCatalogues pc ON pc.PPMPId = a.PPMPId
            //JOIN dbo.Departments d ON d.DepartmentId = a.RequestingOfficeId
            //WHERE YEAR(a.BudgetYear)= 2025 AND a.IsActive = 1 AND pc.IsActive = 1

            var result = await _context.VAppPpmpCatalogues
                .Where(p => p.BudgetYear == budgetYear)
                .Join(approvedPpmps,
                 catalogue => catalogue.Ppmpid,
                 ppmpId => ppmpId,
                 (s, _) => new APPCatalogueDto
                 {
                     PpmpCatalogueId = s.PpmpcatalogueId,
                     Description = s.Description ?? "",
                     ItemCode = s.ItemCode ?? "",
                     ProductCategory = s.ProductCategory,
                     PpmpId = s.Ppmpid,
                     UnitPrice = s.UnitPrice,
                     RequestingOffice = s.RequestingOffice ?? "",
                     UnitOfMeasure = s.UnitOfMeasure ?? "",
                     FirstQty = s.FirstQuarter,
                     SecondQty = s.SecondQuarter,
                     ThirdQty = s.ThirdQuarter,
                     FourthQty = s.FourthQuarter
                 }).ToListAsync();

            return result;
        }

        public async Task<List<APPCatalogueDto>> ViewConsolidatedSuppItems(short budgetYear)
        {
            var approvedPpmps = (await GetOfficesWithApprovedPPMPs(budgetYear)).Select(s => s.PpmpId);

            var result = await _context.VAppPpmpSupplementaryItems
                .Where(p => p.BudgetYear == budgetYear)
                .Join(approvedPpmps,
                 catalogue => catalogue.Ppmpid,
                 ppmpId => ppmpId,
                 (s, _) => new APPCatalogueDto
                 {
                     PpmpCatalogueId = s.PpmpsupplementaryId,
                     Description = s.Description ?? "",
                     ItemCode = s.ItemCode ?? "",
                     ProductCategory = s.ProductCategory,
                     PpmpId = s.Ppmpid,
                     UnitPrice = s.UnitPrice,
                     RequestingOffice = s.RequestingOffice ?? "",
                     UnitOfMeasure = s.UnitOfMeasure ?? "",
                     FirstQty = s.FirstQuarter,
                     SecondQty = s.SecondQuarter,
                     ThirdQty = s.ThirdQuarter,
                     FourthQty = s.FourthQuarter
                 }).ToListAsync();

            return result;
        }

        public async Task<List<APPProjectItemDto>> ViewConsolidatedProjectItems(short budgetYear)
        {
            var approvedPpmps = (await GetOfficesWithApprovedPPMPs(budgetYear)).Select(s => s.PpmpId);

            var result = await _context.VAppPpmpProjectItems
                .Where(p => p.BudgetYear == budgetYear)
                .Join(approvedPpmps,
                 catalogue => catalogue.Ppmpid,
                 ppmpId => ppmpId,
                 (s, _) => new APPProjectItemDto
                 {
                     Cost = s.Cost,
                     Description = s.Description ?? "",
                     PpmpId = s.Ppmpid,
                     ProjectName = s.ProjectName ?? "",
                     Quarter = s.Quarter,
                     RequestingOffice = s.RequestingOffice ?? "",
                     PpmpProjectId = s.PpmpprojectId,
                 }).ToListAsync();

            return result;
        }

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
