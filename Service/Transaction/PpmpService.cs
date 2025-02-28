using EF.Models;
using EF.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Service.Dto.SystemSetup;
using Service.Dto.Transaction;
using Service.Dto.UserManagement;
using Service.Service;
using Service.SystemSetup.Interface;
using Service.Transaction.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Transaction
{
    public class PpmpService : BaseService<Ppmp, PPMPDto>, IPpmpService
    {
        public PpmpService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<Ppmp> IncludeNavigationProperties(IQueryable<Ppmp> query)
        {
            return query.Include(o => o.RequestingOffice);//.Include(i => i.Ppmpcatalogues).Include(i => i.Ppmpsupplementaries);
        }

        protected override IQueryable<Ppmp> ApplySearchFilter(IQueryable<Ppmp> query, string searchQuery)
        {
            return query.Where(p => new[] { p.BudgetYear.ToString(), p.Remarks, p.Status }
                             .Any(value => value != null && value.Contains(searchQuery)));
        }

        public override async Task<PPMPDto> GetByIdAsync(int id)
        {
            var model = await base.GetByIdAsync(id);

            var psdbms = _context.Ppmpcatalogues
                         .Include(pc => pc.Catalogue)
                             .ThenInclude(c => c.MajorCategory)
                         .Include(pc => pc.Catalogue)
                             .ThenInclude(c => c.UnitOfMeasurement)
                         .Include(pc => pc.Catalogue)
                             .ThenInclude(c => c.AccountCode)
                         .Where(x => x.Ppmpid == id && x.IsActive)
                         .ToList();

            var supps = _context.Ppmpsupplementaries
                         .Include(pc => pc.Supplementary)
                             .ThenInclude(c => c.MajorCategory)
                         .Include(pc => pc.Supplementary)
                             .ThenInclude(c => c.UnitOfMeasurement)
                         .Include(pc => pc.Supplementary)
                             .ThenInclude(c => c.AccountCode)
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
                    Id = x.Catalogue.PsdbmcatalogueId,
                    Code = x.Catalogue.Code,
                    Description = x.Catalogue.Description,
                    MajorCategoryName = x.Catalogue.MajorCategory != null ? x.Catalogue.MajorCategory.Name : "",
                    AccountCodeDescription = x.Catalogue.AccountCode != null ? x.Catalogue.AccountCode.Description : "",
                    UnitOfMeasurementCode = x.Catalogue.UnitOfMeasurement != null ? x.Catalogue.UnitOfMeasurement.Code : "",
                },

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
                    Id = x.Supplementary.SupplementaryCatalogueId,
                    Code = x.Supplementary.Code,
                    Description = x.Supplementary.Description,
                    MajorCategoryName = x.Supplementary.MajorCategory != null ? x.Supplementary.MajorCategory.Name : "",
                    AccountCodeDescription = x.Supplementary.AccountCode != null ? x.Supplementary.AccountCode.Description : "",
                    UnitOfMeasurementCode = x.Supplementary.UnitOfMeasurement != null ? x.Supplementary.UnitOfMeasurement.Code : "",
                },
            }).ToList();

            return model;
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
                SubmittedBy = entity.SubmittedByUserId.GetValueOrDefault(),
                SubmittedDate = entity.SubmittedDate.GetValueOrDefault(),
                DeletedBy = entity.DeletedByUserId.GetValueOrDefault(),
                DeletedDate = entity.DeletedDate.GetValueOrDefault(),
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

                Ppmpcatalogues = dto.Ppmpcatalogues.Select(x => new Ppmpcatalogue
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
                }).ToList(),

                Ppmpsupplementaries = dto.Ppmpsupplementaries.Select(x => new Ppmpsupplementary
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
                }).ToList(),
            };

            return entity;
        }

        public IList<int> GetBudgetYears()
        {
            // to do

            return [2023, 2024, 2025, 2026];
        }
    }
}
