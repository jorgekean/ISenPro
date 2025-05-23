﻿using EF;
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

        //protected override IQueryable<UmPerson> IncludeNavigationProperties(IQueryable<UmPerson> query)
        //{
        //    return query.Include(o => o.Department).ThenInclude(s => s.Bureau).Include(o => o.Section).Include(x => x.EmployeeStatusNavigation).Include(x => x.EmployeeStatusNavigation);
        //}

        //protected override IQueryable<UmPerson> ApplySearchFilter(IQueryable<UmPerson> query, string searchQuery)
        //{
        //    return query.Where(p => new[] { p.FirstName, p.LastName }
        //                    .Any(value => value != null && value.Contains(searchQuery)));
        //}

        protected override IQueryable<T> ApplySearchFilter<T>(IQueryable<T> query, string searchQuery)
        {
            // If the type is MyEntity, cast the query and apply filtering.
            if (typeof(T) == typeof(VPersonIndex))
            {
                var typedQuery = query as IQueryable<VPersonIndex>;
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    typedQuery = typedQuery.Where(p => new[] { p.FullName, p.OfficeName, p.SectionName, p.EmployeeStatusLabel }
                             .Any(value => value != null && value.Contains(searchQuery)));
                }

                // Cast back to IQueryable<T> and return
                return typedQuery as IQueryable<T>;
            }

            // Otherwise, for any other type, you can either return the unfiltered query or add your own logic.
            return query;
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
                EmployeeStatusLabel = entity.EmployeeStatusNavigation?.Name,
                EmployeeTitle = entity.EmployeeTitle,
                EmployeeTitleLabel = entity.EmployeeTitleNavigation?.Name,
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
