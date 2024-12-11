using EF;
using EF.Models;
using EF.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Service.Dto.UserManagement;
using Service.Service;
using Service.UserManagement.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UserManagement
{
    public class PersonService : BaseService<UmPerson, PersonDto>, IPersonService
    {
        public PersonService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<UmPerson> IncludeNavigationProperties(IQueryable<UmPerson> query)
        {
            return query.Include(o => o.Department).ThenInclude(s => s.Bureau).Include(o => o.Section);
        }

        protected override IQueryable<UmPerson> ApplySearchFilter(IQueryable<UmPerson> query, string searchQuery)
        {
            return query.Where(p => p.FirstName.Contains(searchQuery) || p.LastName.Contains(searchQuery));
        }

        public override async Task<PersonDto> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            await _context.Entry(entity).Reference(x => x.Department).LoadAsync();

            if (entity != null)
            {
                if (entity.Department != null)
                {
                    await _context.Entry(entity.Department).Reference(x => x.Bureau).LoadAsync();

                    if (entity.Department.Bureau != null)
                    {
                        await _context.Entry(entity.Department.Bureau).Reference(x => x.Division).LoadAsync();
                    }
                }

              

                return MapToDto(entity);
            }
            return null;
        }

        protected override PersonDto MapToDto(UmPerson entity)
        {
            var dto = new PersonDto
            {
                Id = entity.PersonId,
                LastName = entity.LastName,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                DepartmentId = entity.DepartmentId,
                Designation = entity.Designation,
                Address = entity.Address,
                ContactNo = entity.ContactNo,
                Email = entity.Email,
                EmployeeStatus = entity.EmployeeStatus,
                EmployeeTitle = entity.EmployeeTitle,
                IsHeadOfOffice = entity.IsHeadOfOffice,
                Remarks = entity.Remarks,
                SectionId = entity.SectionId,
                Thumbnail = entity.Thumbnail,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                Department = entity.Department == null ? null : new DepartmentDto
                {
                    Id = entity.Department.DepartmentId,
                    Code = entity.Department.Code,
                    Name = entity.Department.Name,
                    Description = entity.Department.Description,
                    IsActive = entity.Department.IsActive,
                    CreatedDate = entity.Department.CreatedDate,
                    CreatedBy = entity.Department.CreatedByUserId,
                    Bureau = entity.Department.Bureau == null ? null : new BureauDto
                    {
                        Id = entity.Department.Bureau.BureauId,
                        Code = entity.Department.Bureau.Code,
                        Description = entity.Department.Bureau.Description,
                        Name = entity.Department.Bureau.Name,
                    }
                },
                Section = entity.Section == null ? null : new SectionDto
                {
                    Id = entity.Section.SectionId,
                    Code = entity.Section.Code,
                    Name = entity.Section.Name,
                    Description = entity.Section.Description,
                    IsActive = entity.Section.IsActive,
                    CreatedDate = entity.Section.CreatedDate,
                    CreatedBy = entity.Section.CreatedByUserId
                },
                BureauId = entity.Department?.BureauId,
                DivisionId = entity?.Department?.Bureau?.DivisionId
            };
            return dto;
        }

        protected override UmPerson MapToEntity(PersonDto dto)
        {
            var entity = new UmPerson
            {
                PersonId = dto.Id.GetValueOrDefault(),
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                Address = dto.Address,
                ContactNo = dto.ContactNo,
                DepartmentId = dto.DepartmentId,
                Designation = dto.Designation,
                Email = dto.Email,
                EmployeeStatus = dto.EmployeeStatus,
                EmployeeTitle = dto.EmployeeTitle,
                IsHeadOfOffice = dto.IsHeadOfOffice,
                Remarks = dto.Remarks,
                SectionId = dto.SectionId,
                Thumbnail = dto.Thumbnail,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy
            };
            return entity;
        }
    }
}
