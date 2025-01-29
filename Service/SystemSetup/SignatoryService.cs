using EF;
using EF.Models;
using EF.Models.SystemSetup;
using EF.Models.UserManagement;
using Service.Dto.SystemSetup;
using Service.Dto.UserManagement;
using Service.Service;
using Service.SystemSetup.Interface;
using Service.UserManagement.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service.SystemSetup
{
    public class SignatoryService : BaseService<SsSignatory, SignatoryDto>, ISignatoryService
    {
        public SignatoryService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<SsSignatory> IncludeNavigationProperties(IQueryable<SsSignatory> query)
        {
            return query.Include(o => o.Person).ThenInclude(s => s.Department);
        }

        protected override IQueryable<SsSignatory> ApplySearchFilter(IQueryable<SsSignatory> query, string searchQuery)
        {
            return query.Where(p => new[] { p.SignatoryDesignation.Name, p.ReportSection.Name }
                        .Any(value => value != null && value.Contains(searchQuery)));
        }
        public override async Task<SignatoryDto> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            await _context.Entry(entity).Reference(x => x.Person).LoadAsync();

            if (entity != null)
            {
                if (entity.Person != null)
                {
                    await _context.Entry(entity.Person).Reference(x => x.Department).LoadAsync();
                }

                return MapToDto(entity);
            }
            return null;
        }

        protected override SignatoryDto MapToDto(SsSignatory entity)
        {
            var dto = new SignatoryDto();

            dto.Id = entity.SignatoryId;
            dto.Transactions = entity.Transactions ?? (int?)null;
            dto.ModuleName = entity.Transactions.HasValue ? _context.UmModules.FirstOrDefault(x => x.ModuleId == entity.Transactions.Value).Name : string.Empty;
            dto.Sequence = entity.Sequence;
            dto.SignatoryDesignationId = entity.SignatoryDesignationId ?? (int?)null;
            dto.SignatoryDesignation = entity.SignatoryDesignationId.HasValue ? _context.SsReferenceTables.FirstOrDefault(x => x.ReferenceTableId == entity.SignatoryDesignationId.Value).Name : string.Empty;
            dto.SignatoryOfficeId = entity.SignatoryOfficeId ?? (int?)null;
            dto.SignatoryOffice = entity.SignatoryOfficeId.HasValue ? _context.SsReferenceTables.FirstOrDefault(x => x.ReferenceTableId == entity.SignatoryOfficeId.Value).Name : string.Empty;
            dto.ReportSectionId = entity.ReportSectionId ?? (int?)null;
            dto.WithCondition = entity.WithCondition ?? (bool?)null;
            dto.MaximumAmount = entity.MaximumAmount ?? (double?)null;
            dto.MinimumAmount = entity.MinimumAmount ?? (double?)null;
            dto.PersonId = entity.PersonId ?? (int?)null;
            dto.IsActive = entity.IsActive;

            dto.Person = new PersonDto
            {
                Id = entity.PersonId,
                LastName = entity.Person.LastName,
                FirstName = entity.Person.FirstName,
                MiddleName = entity.Person.MiddleName,
                DepartmentId = entity.Person.DepartmentId,
                Designation = entity.Person.Designation,
                Address = entity.Person.Address,
                ContactNo = entity.Person.ContactNo,
                Email = entity.Person.Email,
                EmployeeStatus = entity.Person.EmployeeStatus,
                EmployeeTitle = entity.Person.EmployeeTitle,
                IsHeadOfOffice = entity.Person.IsHeadOfOffice,
                Remarks = entity.Person.Remarks,
                SectionId = entity.Person.SectionId,

                Department = entity.Person.Department == null ? null : new DepartmentDto
                {
                    Id = entity.Person.Department.DepartmentId,
                    Code = entity.Person.Department.Code,
                    Name = entity.Person.Department.Name,
                }
            };

            return dto;
        }

        protected override SsSignatory MapToEntity(SignatoryDto dto)
        {
            var entity = new SsSignatory
            {
                SignatoryId = dto.Id.GetValueOrDefault(),
                Sequence = dto.Sequence,
                Transactions = dto.Transactions,
                SignatoryDesignationId = dto.SignatoryDesignationId,
                SignatoryOfficeId = dto.SignatoryOfficeId,
                ReportSectionId = dto.ReportSectionId,
                WithCondition = dto.WithCondition,
                MaximumAmount = dto.MaximumAmount,
                MinimumAmount = dto.MinimumAmount,
                PersonId = dto.PersonId,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedByUserId = 1,
            };

            return entity;
        }
    }
}
