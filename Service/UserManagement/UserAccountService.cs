using EF;
using EF.Models;
using EF.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Service.Dto.UserManagement;
using Service.Service;
using Service.UserManagement.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UserManagement
{
    public class UserAccountService : BaseService<UmUserAccount, UserAccountDto>, IUserAccountService
    {
        public UserAccountService(ISenProContext context) : base(context)
        {
        }

        //protected override IQueryable<UmUserAccount> IncludeNavigationProperties(IQueryable<UmUserAccount> query)
        //{
        //    return query.Include(o => o.Person).ThenInclude(s => s.Department).Include(o => o.Role);
        //}

        //protected override IQueryable<UmUserAccount> ApplySearchFilter(IQueryable<UmUserAccount> query, string searchQuery)
        //{
        //    return query.Where(p => new[] { p.UserId, p.Person.LastName, p.Person.FirstName, p.Role.Name, p.Person.Department.Name }
        //                    .Any(value => value != null && value.Contains(searchQuery)));
        //}

        protected override IQueryable<T> ApplySearchFilter<T>(IQueryable<T> query, string searchQuery)
        {
            // If the type is MyEntity, cast the query and apply filtering.
            if (typeof(T) == typeof(VUserAccountIndex))
            {
                var typedQuery = query as IQueryable<VUserAccountIndex>;
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    typedQuery = typedQuery.Where(p => new[] { p.FullName, p.OfficeName, p.SectionName, p.EmployeeStatusLabel, p.RoleName }
                             .Any(value => value != null && value.Contains(searchQuery)));
                }

                // Cast back to IQueryable<T> and return
                return typedQuery as IQueryable<T>;
            }

            // Otherwise, for any other type, you can either return the unfiltered query or add your own logic.
            return query;
        }

        public override async Task<UserAccountDto> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            await _context.Entry(entity).Reference(x => x.Person).LoadAsync();
            await _context.Entry(entity).Reference(x => x.Role).LoadAsync();

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

        protected override UserAccountDto MapToDto(UmUserAccount entity)
        {
            var dto = new UserAccountDto
            {
                Id = entity.UserAccountId,
                CreatedBy = entity.CreatedByUserId,
                CreatedDate = entity.CreatedDate,
                ExpireDate = entity.ExpireDate,
                IsActive = entity.IsActive,
                IsAdmin = entity.IsAdmin,
                Password = entity.Password,
                PersonId = entity.PersonId,
                RoleId = entity.RoleId,
                UserId = entity.UserId,

                Person = new PersonDto
                {
                    Id = entity.Person.PersonId,
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
                },

                Role = entity.Role == null ? null : new RoleDto
                {
                    Id = entity.Role.RoleId,
                    Name = entity.Role.Name,
                    Description = entity.Role.Description,
                }
            };
            return dto;
        }

        protected override UmUserAccount MapToEntity(UserAccountDto dto)
        {
            var entity = new UmUserAccount
            {
                UserAccountId = dto.Id.GetValueOrDefault(),
                ExpireDate = dto.ExpireDate,
                IsAdmin = dto.IsAdmin,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                PersonId = dto.PersonId,
                RoleId = dto.RoleId,
                UserId = dto.UserId,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy
            };
            return entity;
        }
    }
}
