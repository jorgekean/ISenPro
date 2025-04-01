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
    public class PpmpService : BaseService<Ppmp, PPMPDto>, IPpmpService
    {
        private readonly CachedItems _cachedItems;
        public PpmpService(ISenProContext context,
            CachedItems cachedItems) : base(context)
        {
            _cachedItems = cachedItems;
        }

        #region CRUD
        protected override IQueryable<T> ApplySearchFilter<T>(IQueryable<T> query, string searchQuery)
        {
            // If the type is MyEntity, cast the query and apply filtering.
            if (typeof(T) == typeof(VPpmpindex))
            {
                var typedQuery = query as IQueryable<VPpmpindex>;
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    typedQuery = typedQuery.Where(p => new[] { p.BudgetYear.ToString(), p.Remarks, p.Status, p.Ppmpno, p.OfficeName, p.PreparedBy }
                             .Any(value => value != null && value.Contains(searchQuery)));
                }

                // Cast back to IQueryable<T> and return
                return typedQuery as IQueryable<T>;
            }

            // Otherwise, for any other type, you can either return the unfiltered query or add your own logic.
            return query;
        }

        public override async Task<PPMPDto> GetByIdAsync(int id)
        {
            var model = await base.GetByIdAsync(id);

            var psdbms = _context.VPpmpPsdbmcatalogues
                         .Where(x => x.Ppmpid == id && x.IsActive)
                         .ToList();

            var supps = _context.VPpmpSupplementaryCatalogues
                         .Where(x => x.Ppmpid == id && x.IsActive)
                         .ToList();

            var projects = _context.Ppmpprojects.Include(i => i.AccountCode)
                         .Where(x => x.Ppmpid == id && x.IsActive)
                         .ToList();

            model.Ppmpcatalogues = psdbms.Select(x => new PPMPCatalogueDto
            {
                Id = x.PpmpcatalogueId,
                Ppmpid = x.Ppmpid,
                CatalogueId = x.CatalogueId,
                FirstQuarter = x.FirstQuarter,
                SecondQuarter = x.SecondQuarter,
                ThirdQuarter = x.ThirdQuarter,
                FourthQuarter = x.FourthQuarter,
                UnitPrice = x.UnitPrice,
                Amount = x.Amount,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedByUserId,
                UpdatedDate = x.UpdatedDate,
                Updatedby = x.UpdatedByUserId,
                DeletedDate = x.DeletedDate,
                DeletedBy = x.DeletedByUserId,
                Description = x.Description,
                Remarks = x.Remarks,

                Catalogue = new PSDBMCatalogueDto
                {
                    Id = x.CatalogueId,
                    Code = x.CatalogueCode,
                    Description = x.Description,
                    MajorCategoryName = x.MajorCategoryName,
                    AccountCodeDescription = x.AccountCodeDescription,
                    UnitOfMeasurementCode = x.UnitOfMeasurementCode
                }

            }).ToList();

            model.Ppmpsupplementaries = supps.Select(x => new PPMPSupplementariesDto
            {
                Id = x.PpmpsupplementaryId,
                Ppmpid = x.Ppmpid,
                SupplementaryId = x.SupplementaryId,
                FirstQuarter = x.FirstQuarter,
                SecondQuarter = x.SecondQuarter,
                ThirdQuarter = x.ThirdQuarter,
                FourthQuarter = x.FourthQuarter,
                UnitPrice = x.UnitPrice,
                Amount = x.Amount,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedByUserId,
                UpdatedDate = x.UpdatedDate,
                Updatedby = x.UpdatedByUserId,
                DeletedDate = x.DeletedDate,
                DeletedBy = x.DeletedByUserId,
                Description = x.Description,
                Remarks = x.Remarks,

                Supplementary = new SupplementaryCatalogueDto
                {
                    Id = x.SupplementaryId,
                    Code = x.CatalogueCode,
                    Description = x.Description,
                    MajorCategoryName = x.MajorCategoryName,
                    AccountCodeDescription = x.AccountCodeDescription,
                    UnitOfMeasurementCode = x.UnitOfMeasurementCode
                },
            }).ToList();

            model.PpmpProjects = projects.Select(x => new PPMPProjectDto
            {
                Id = x.PpmpprojectId,
                Ppmpid = x.Ppmpid,
                ProjectName = x.ProjectName,
                Description = x.Description,
                Quarter = x.Quarter,
                Cost = x.Cost,
                ProjectStatus = x.ProjectStatus,
                IsActive = x.IsActive,
                CreatedBy = x.CreatedByUserId,
                CreatedDate = x.CreatedDate,
                AccountCode = x.AccountCode != null ? new AccountCodeDto
                {
                    Code = x.AccountCode.Code,
                    Description = x.AccountCode.Description,
                    Id = x.AccountCode.AccountCodeId
                } : null,
            }).ToList();

            return model;
        }

        public override async Task<object> AddAsync(PPMPDto dto)
        {
            dto.Ppmpno = GeneratePppmpNo(dto.BudgetYear);

            return await base.AddAsync(dto);
        }

        // Expect to received the updated/added ppmpcatalogues and ppmpsupplementaries ONLY
        public override async Task UpdateAsync(PPMPDto dto)
        {
            var entity = MapToEntity(dto);

            // update the PPMP model
            _dbSet.Update(entity);

            #region PPMP Catalogues
            // Get a list of IDs from the DTO for records that are NOT new.
            var existingIds = dto.Ppmpcatalogues
                                 .Where(item => item.Id != 0)
                                 .Select(item => item.Id)
                                 .ToList();

            // Load all existing Ppmpcatalogues in one go.
            var existingCatalogues = await _context.Ppmpcatalogues
                .Where(c => existingIds.Contains(c.PpmpcatalogueId))
                .ToDictionaryAsync(c => c.PpmpcatalogueId);

            foreach (var item in dto.Ppmpcatalogues)
            {
                if (item.Id != 0 && existingCatalogues.TryGetValue(item.Id, out var catalogue))
                {
                    // Existing record: update its properties.
                    catalogue.Ppmpid = dto.Id;
                    catalogue.CatalogueId = item.CatalogueId;
                    catalogue.FirstQuarter = item.FirstQuarter;
                    catalogue.SecondQuarter = item.SecondQuarter;
                    catalogue.ThirdQuarter = item.ThirdQuarter;
                    catalogue.FourthQuarter = item.FourthQuarter;
                    catalogue.UnitPrice = item.UnitPrice;
                    catalogue.Amount = item.Amount;
                    catalogue.IsActive = item.IsActive;
                    catalogue.CreatedDate = item.CreatedDate;
                    catalogue.CreatedByUserId = item.CreatedBy;
                    catalogue.UpdatedDate = item.UpdatedDate;
                    catalogue.UpdatedByUserId = item.Updatedby;
                    catalogue.DeletedDate = item.DeletedDate;
                    catalogue.DeletedByUserId = item.DeletedBy;
                    catalogue.Description = item.Description;
                    catalogue.Remarks = item.Remarks;
                }
                else
                {
                    // New record: create a new instance and add it to the context.
                    catalogue = new Ppmpcatalogue
                    {
                        Ppmpid = dto.Id,
                        CatalogueId = item.CatalogueId,
                        FirstQuarter = item.FirstQuarter,
                        SecondQuarter = item.SecondQuarter,
                        ThirdQuarter = item.ThirdQuarter,
                        FourthQuarter = item.FourthQuarter,
                        UnitPrice = item.UnitPrice,
                        Amount = item.Amount,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedByUserId = 1, // TO DO: Use the current user's id
                        Description = item.Description,
                        Remarks = item.Remarks
                    };
                    _context.Ppmpcatalogues.Add(catalogue);
                }
            }
            #endregion

            #region PPMP Supplementaries
            // Get a list of IDs from the DTO for records that are NOT new.
            var existingSuppIds = dto.Ppmpsupplementaries
                                 .Where(item => item.Id != 0)
                                 .Select(item => item.Id)
                                 .ToList();

            // Load all existing PpmpSupplementaries in one go.
            var existingSupps = await _context.Ppmpsupplementaries
                .Where(c => existingSuppIds.Contains(c.PpmpsupplementaryId))
                .ToDictionaryAsync(c => c.PpmpsupplementaryId);

            foreach (var item in dto.Ppmpsupplementaries)
            {
                if (item.Id != 0 && existingSupps.TryGetValue(item.Id, out var supplementary))
                {
                    // Existing record: update its properties.
                    supplementary.Ppmpid = dto.Id;
                    supplementary.SupplementaryId = item.SupplementaryId;
                    supplementary.FirstQuarter = item.FirstQuarter;
                    supplementary.SecondQuarter = item.SecondQuarter;
                    supplementary.ThirdQuarter = item.ThirdQuarter;
                    supplementary.FourthQuarter = item.FourthQuarter;
                    supplementary.UnitPrice = item.UnitPrice;
                    supplementary.Amount = item.Amount;
                    supplementary.IsActive = item.IsActive;
                    supplementary.CreatedDate = item.CreatedDate;
                    supplementary.CreatedByUserId = item.CreatedBy;
                    supplementary.UpdatedDate = item.UpdatedDate;
                    supplementary.UpdatedByUserId = item.Updatedby;
                    supplementary.DeletedDate = item.DeletedDate;
                    supplementary.DeletedByUserId = item.DeletedBy;
                    supplementary.Description = item.Description;
                    supplementary.Remarks = item.Remarks;
                }
                else
                {
                    // New record: create a new instance and add it to the context.
                    supplementary = new Ppmpsupplementary
                    {
                        Ppmpid = dto.Id,
                        SupplementaryId = item.SupplementaryId,
                        FirstQuarter = item.FirstQuarter,
                        SecondQuarter = item.SecondQuarter,
                        ThirdQuarter = item.ThirdQuarter,
                        FourthQuarter = item.FourthQuarter,
                        UnitPrice = item.UnitPrice,
                        Amount = item.Amount,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedByUserId = 1, // TO DO: Use the current user's id
                        Description = item.Description,
                        Remarks = item.Remarks
                    };
                    _context.Ppmpsupplementaries.Add(supplementary);
                }
            }
            #endregion

            #region PPMP Projects
            // Get a list of IDs from the DTO for records that are NOT new.
            var existingProjectIds = dto.PpmpProjects
                                 .Where(item => item.Id != 0)
                                 .Select(item => item.Id)
                                 .ToList();

            // Load all existing PpmpSupplementaries in one go.
            var existingProjs = await _context.Ppmpprojects
                .Where(c => existingProjectIds.Contains(c.PpmpprojectId))
                .ToDictionaryAsync(c => c.PpmpprojectId);

            foreach (var item in dto.PpmpProjects)
            {
                if (item.Id != 0 && existingProjs.TryGetValue(item.Id, out var project))
                {
                    // Existing record: update its properties.
                    project.PpmpprojectId = item.Id;
                    project.Ppmpid = item.Ppmpid;
                    project.ProjectName = item.ProjectName;
                    project.Description = item.Description;
                    project.Quarter = item.Quarter;
                    project.Cost = item.Cost;
                    project.ProjectStatus = item.ProjectStatus;
                    project.IsActive = item.IsActive;
                    project.CreatedByUserId = item.CreatedBy;
                    project.CreatedDate = item.CreatedDate;
                    project.AccountCodeId = item.AccountCode?.Id;
                }
                else
                {
                    // New record: create a new instance and add it to the context.
                    project = new Ppmpproject
                    {
                        PpmpprojectId = item.Id,
                        Ppmpid = item.Ppmpid,
                        ProjectName = item.ProjectName,
                        Description = item.Description,
                        Quarter = item.Quarter,
                        Cost = item.Cost,
                        ProjectStatus = item.ProjectStatus,
                        IsActive = item.IsActive,
                        CreatedByUserId = item.CreatedBy,
                        CreatedDate = item.CreatedDate,
                        AccountCodeId = item.AccountCode?.Id
                    };
                    _context.Ppmpprojects.Add(project);
                }
            }
            #endregion

            await _context.SaveChangesAsync();
        }

        protected override PPMPDto MapToDto(Ppmp entity)
        {
            var dto = new PPMPDto
            {
                Id = entity.Ppmpid,
                BudgetYear = entity.BudgetYear,
                Remarks = entity.Remarks,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                AdditionalInflationValue = entity.AdditionalInflationValue,
                AdditionalTenPercent = entity.AdditionalTenPercent,
                CatalogueAmount = entity.CatalogueAmount,
                SupplementaryAmount = entity.SupplementaryAmount,
                TotalAmount = entity.TotalAmount,
                GrandTotalAmount = entity.GrandTotalAmount,
                ProjectAmount = entity.ProjectAmount,
                IsActive = entity.IsActive,
                IsSubmitted = entity.IsSubmitted,
                Ppmpno = entity.Ppmpno,
                RequestingOfficeId = entity.RequestingOfficeId,
                SubmittedBy = entity.SubmittedByUserId,
                SubmittedDate = entity.SubmittedDate,
                DeletedBy = entity.DeletedByUserId,
                DeletedDate = entity.DeletedDate,
                IsDeleted = entity.DeletedDate.HasValue,
                RequestingOffice = entity.RequestingOffice != null ? new DepartmentDto
                {
                    Name = entity.RequestingOffice.Name,
                    Code = entity.RequestingOffice.Code,
                    Description = entity.RequestingOffice.Description,
                } : null,


            };
            return dto;
        }

        protected override Ppmp MapToEntity(PPMPDto dto)
         {
            var entity = new Ppmp
            {
                Ppmpid = dto.Id,
                Ppmpno = dto.Ppmpno,
                BudgetYear = dto.BudgetYear,
                Remarks = dto.Remarks,
                Status = dto.Status,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy,
                IsActive = dto.IsActive,
                IsSubmitted = dto.IsSubmitted,
                SubmittedByUserId = dto.SubmittedBy,
                SubmittedDate = dto.SubmittedDate,
                DeletedByUserId = dto.DeletedBy,
                DeletedDate = dto.DeletedDate,
                UpdatedDate = dto.UpdatedDate,
                UpdatedByUserId = dto.Updatedby,
                AdditionalInflationValue = dto.AdditionalInflationValue,
                AdditionalTenPercent = dto.AdditionalTenPercent,
                CatalogueAmount = dto.CatalogueAmount,
                SupplementaryAmount = dto.SupplementaryAmount,
                TotalAmount = dto.TotalAmount,
                GrandTotalAmount = dto.GrandTotalAmount,
                ProjectAmount = dto.ProjectAmount,
                RequestingOfficeId = dto.RequestingOfficeId,

                // Populate PpmpCatalogues for Create ONLY(has PPmpId)
                Ppmpcatalogues = dto.Id == 0 ? dto.Ppmpcatalogues.Select(x => new Ppmpcatalogue
                {
                    PpmpcatalogueId = x.Id,
                    Ppmpid = x.Ppmpid,
                    CatalogueId = x.CatalogueId,
                    FirstQuarter = x.FirstQuarter,
                    SecondQuarter = x.SecondQuarter,
                    ThirdQuarter = x.ThirdQuarter,
                    FourthQuarter = x.FourthQuarter,
                    UnitPrice = x.UnitPrice,
                    Amount = x.Amount,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    CreatedByUserId = x.CreatedBy,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedByUserId = x.Updatedby,
                    DeletedDate = x.DeletedDate,
                    DeletedByUserId = x.DeletedBy,
                    Description = x.Description,
                    Remarks = x.Remarks
                }).ToList() : [],

                Ppmpsupplementaries = dto.Id == 0 ? dto.Ppmpsupplementaries.Select(x => new Ppmpsupplementary
                {
                    PpmpsupplementaryId = x.Id,
                    Ppmpid = x.Ppmpid,
                    SupplementaryId = x.SupplementaryId,
                    FirstQuarter = x.FirstQuarter,
                    SecondQuarter = x.SecondQuarter,
                    ThirdQuarter = x.ThirdQuarter,
                    FourthQuarter = x.FourthQuarter,
                    UnitPrice = x.UnitPrice,
                    Amount = x.Amount,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    CreatedByUserId = x.CreatedBy,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedByUserId = x.Updatedby,
                    DeletedDate = x.DeletedDate,
                    DeletedByUserId = x.DeletedBy,
                    Description = x.Description,
                    Remarks = x.Remarks
                }).ToList() : [],

                Ppmpprojects = dto.Id == 0 ? dto.PpmpProjects.Select(x => new Ppmpproject
                {
                    PpmpprojectId = x.Id,
                    Ppmpid = x.Ppmpid,
                    ProjectName = x.ProjectName,
                    Description = x.Description,
                    Quarter = x.Quarter,
                    Cost = x.Cost,
                    ProjectStatus = x.ProjectStatus,
                    IsActive = x.IsActive,
                    CreatedByUserId = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    AccountCodeId = x.AccountCode?.Id
                }).ToList() : []
            };

            return entity;
        }

        #endregion

        #region Reports
        //  Implement the PPMPGenerateReport method here
        public async Task GenerateReport(int ppmpId)
        {
            string templatePath = "C:\\Users\\Jorge\\Documents\\Freelance\\PPMS\\Report Templates\\PpmpReportTemplateMergeFieldWithChart.docx";
            string outputPath = $"C:\\Users\\Jorge\\Documents\\Freelance\\PPMS\\Report Output\\PPMP_Output_{DateTime.Now.ToString("MMddyyyyHHmmss")}.docx";

            // Load the PPMP record
            var ppmp = await _context.Ppmps
                .Include(p => p.RequestingOffice)
                .FirstOrDefaultAsync(p => p.Ppmpid == ppmpId);

            // Load the PPMP Catalogues
            var ppmpCatalogues = await _context.Ppmpcatalogues
                .Include(c => c.Catalogue)
                .Where(c => c.Ppmpid == ppmpId)
                .ToListAsync();

            // Load the PPMP Supplementaries
            var ppmpSupplementaries = await _context.Ppmpsupplementaries
                .Include(s => s.Supplementary)
                .Where(s => s.Ppmpid == ppmpId)
                .ToListAsync();

            // Load the PPMP Projects
            var ppmpProjects = await _context.Ppmpprojects
                .Include(p => p.AccountCode)
                .Where(p => p.Ppmpid == ppmpId)
                .ToListAsync();

            // Prepare the placeholders
            var placeholders = new Dictionary<string, string>
            {
                { BaseGenerateReport.MERGEFIELD_BUDGETYEAR, ppmp.BudgetYear.ToString() },
                { BaseGenerateReport.MERGEFIELD_DEPARTMENTNAME, ppmp.RequestingOffice.Name }
            };

            var generator = new PPMPGenerateReport();
            generator.Items = ppmpCatalogues.Select(c => new PPMPItem
            {
                Description = c.Catalogue.Description,
                Qtr1 = c.FirstQuarter,
                Qtr2 = c.SecondQuarter,
                Qtr3 = c.ThirdQuarter,
                Qtr4 = c.FourthQuarter,
                Amount = c.Amount,
                Remarks = c.Remarks,
                UnitPrice = c.UnitPrice,
                //UOM = c.
            }).ToList();
            generator.GenerateReport(templatePath, outputPath, placeholders);

            // Replace the table placeholders
            //ReplaceTablePlaceholders(outputPath, ppmpCatalogues, ppmpSupplementaries, ppmpProjects);
        }
        #endregion

        public IList<int> GetBudgetYears()
        {
            // to do

            return [2023, 2024, 2025, 2026];
        }

        #region private methods

        private string GeneratePppmpNo(short budgetYear)
        {
            // Retrieve the last PPMP record for the given budget year
            var ppmpLastRecord = _context.Ppmps
                .Where(ppmp => ppmp.BudgetYear == budgetYear)
                .OrderByDescending(ppmp => ppmp.Ppmpid)
                .FirstOrDefault();

            string referenceNo = ppmpLastRecord?.Ppmpno ?? $"PPMP-{budgetYear}-0000";

            // Split reference number
            var splitReferenceNo = referenceNo.Split('-');

            // Ensure the year is up-to-date
            splitReferenceNo[1] = budgetYear.ToString(CultureInfo.InvariantCulture);

            // Increment the series number or reset if it's a new year
            int seriesNumber = (splitReferenceNo[1] == budgetYear.ToString(CultureInfo.InvariantCulture))
                ? int.Parse(splitReferenceNo[2]) + 1
                : 1;

            splitReferenceNo[2] = seriesNumber.ToString("D4"); // Ensures 4-digit formatting

            return string.Join("-", splitReferenceNo);
        }   
        #endregion
    }
}
