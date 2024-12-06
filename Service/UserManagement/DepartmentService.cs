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
    public class DepartmentService : BaseService<UmDepartment, DepartmentDto>, IDepartmentService
    {
        public DepartmentService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<UmDepartment> IncludeNavigationProperties(IQueryable<UmDepartment> query)
        {
            return query.Include(o => o.Bureau);
        }

        protected override IQueryable<UmDepartment> ApplySearchFilter(IQueryable<UmDepartment> query, string searchQuery)
        {
            return query.Where(p => p.Name.Contains(searchQuery) || p.Code.Contains(searchQuery) || (string.IsNullOrWhiteSpace(p.Description) || p.Description.Contains(searchQuery)));
        }

        protected override DepartmentDto MapToDto(UmDepartment entity)
        {
            var dto = new DepartmentDto
            {
                Id = entity.DepartmentId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                BureauId = entity.BureauId,
                ResponsibilityCenter = entity.ResponsibilityCenter,
                Bureau = entity.Bureau == null ? null : new BureauDto
                {
                    Id = entity.Bureau.BureauId,
                    Code = entity.Bureau.Code,
                    Name = entity.Bureau.Name,
                    Description = entity.Bureau.Description,
                    IsActive = entity.Bureau.IsActive,
                    CreatedDate = entity.Bureau.CreatedDate,
                    CreatedBy = entity.Bureau.CreatedByUserId
                }

            };
            return dto;
        }

        protected override UmDepartment MapToEntity(DepartmentDto dto)
        {
            var entity = new UmDepartment
            {
                DepartmentId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy,
                BureauId = dto.BureauId,
                ResponsibilityCenter = dto.ResponsibilityCenter               
            };
            return entity;
        }
    }
}
