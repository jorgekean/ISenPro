using EF;
using EF.Models;
using EF.Models.UserManagement;
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
    public class RoleService : BaseService<UmRole, RoleDto>, IRoleService
    {
        public RoleService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<UmRole> ApplySearchFilter(IQueryable<UmRole> query, string searchQuery)
        {
            return query.Where(p => new[] { p.Name, p.Code, p.Description }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        protected override RoleDto MapToDto(UmRole entity)
        {
            var dto = new RoleDto
            {
                Id = entity.RoleId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy
            };
            return dto;
        }

        protected override UmRole MapToEntity(RoleDto dto)
        {
            var entity = new UmRole
            {
                RoleId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedBy = dto.CreatedBy
            };
            return entity;
        }
    }
}
