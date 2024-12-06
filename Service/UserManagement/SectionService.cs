using EF;
using EF.Models;
using EF.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
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
    public class SectionService : BaseService<UmSection, SectionDto>, ISectionService
    {
        public SectionService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<UmSection> IncludeNavigationProperties(IQueryable<UmSection> query)
        {
            return query.Include(o => o.Department);
        }
        protected override IQueryable<UmSection> ApplySearchFilter(IQueryable<UmSection> query, string searchQuery)
        {
            return query.Where(p => p.Name.Contains(searchQuery) || p.Code.Contains(searchQuery) || (string.IsNullOrWhiteSpace(p.Description) || p.Description.Contains(searchQuery)));
        }

        protected override SectionDto MapToDto(UmSection entity)
        {
            var dto = new SectionDto
            {
                Id = entity.SectionId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                DepartmentId = entity.DepartmentId,
                Department = entity.Department == null ? null : new DepartmentDto
                {
                    Id = entity.Department.DepartmentId,
                    Code = entity.Department.Code,
                    Name = entity.Department.Name,
                    Description = entity.Department.Description,
                    IsActive = entity.Department.IsActive,
                    CreatedDate = entity.Department.CreatedDate,
                    CreatedBy = entity.Department.CreatedByUserId
                }
            };
            return dto;
        }

        protected override UmSection MapToEntity(SectionDto dto)
        {
            var entity = new UmSection
            {
                SectionId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy,
                DepartmentId = dto.DepartmentId               
            };
            return entity;
        }
    }
}
