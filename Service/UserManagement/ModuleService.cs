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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Service.UserManagement
{
    public class ModuleService : BaseService<UmModule, ModuleDto>, IModuleService
    {
        public ModuleService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<UmModule> IncludeNavigationProperties(IQueryable<UmModule> query)
        {
            return query.Include(o => o.Page).Include(o => o.UmModuleControls);
        }


        protected override IQueryable<UmModule> ApplySearchFilter(IQueryable<UmModule> query, string searchQuery)
        {
            return query.Where(p => p.Name.Contains(searchQuery) || p.Code.Contains(searchQuery) || (string.IsNullOrWhiteSpace(p.Description) || p.Description.Contains(searchQuery)));
        }

        protected override ModuleDto MapToDto(UmModule entity)
        {
            var dto = new ModuleDto
            {
                Id = entity.ModuleId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                PageId = entity.PageId,
                PageName = entity.Page?.PageName,
                ParentModuleId = entity.ParentModuleId,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                ModuleControls = entity.UmModuleControls.Select(x => new ModuleControlDto
                {
                    Id = x.ModuleControlId,
                    ModuleId = x.ModuleId,
                    ControlId = x.ControlId,
                    ControlName = x.Control?.ControlName,
                    IsChecked = x.IsChecked,
                    ModuleName = x.Module?.Name,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedByUserId
                })
            };
            return dto;
        }

        protected override UmModule MapToEntity(ModuleDto dto)
        {
            var entity = new UmModule
            {
                ModuleId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                PageId = dto.PageId,
                ParentModuleId = dto.ParentModuleId,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy,


                UmModuleControls = dto.ModuleControls.Select(x => new UmModuleControl
                {
                    ModuleControlId = x.Id.GetValueOrDefault(),
                    ModuleId = x.ModuleId.GetValueOrDefault(),
                    ControlId = x.ControlId,
                    IsChecked = x.IsChecked,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    CreatedByUserId = x.CreatedBy
                }).ToList()
            };
            return entity;
        }

        public async Task<IEnumerable<PageDto>> GetAllPagesAsync()
        {
            var entities = await _context.UmPages.ToListAsync();

            return entities.Select(s => new PageDto
            {
                Id = s.PageId,
                PageName = s.PageName,
                Description = s.Description
            }).ToList();
        }

        public async Task<IEnumerable<ControlDto>> GetAllControlsAsync()
        {

            var entities = await _context.UmControls.ToListAsync();

            return entities.Select(s => new ControlDto
            {
                Id = s.ControlId,
                ControlName = s.ControlName,
                Description = s.Description
            }).ToList();
        }
    }
}
