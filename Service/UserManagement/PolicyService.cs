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
    public class PolicyService : BaseService<UmPolicy, PolicyDto>, IPolicyService
    {
        public PolicyService(ISenProContext context) : base(context)
        {
        }

        //protected override IQueryable<UmPolicy> IncludeNavigationProperties(IQueryable<UmPolicy> query)
        //{
        //    //return query.Include(o => o.P).Include(o => o.UmPolicyControls);
        //}


        protected override IQueryable<UmPolicy> ApplySearchFilter(IQueryable<UmPolicy> query, string searchQuery)
        {
            return query.Where(p => new[] { p.Name, p.Code, p.Description }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        //public override async Task<PolicyDto> GetByIdAsync(int id)
        //{
        //    var entity = await _dbSet.FindAsync(id);

        //    await _context.Entry(entity).Collection(x => x.).LoadAsync();

        //    if (entity != null)
        //    {
        //        return MapToDto(entity);
        //    }                

        //    return null;
        //}

        protected override PolicyDto MapToDto(UmPolicy entity)
        {
            var dto = new PolicyDto
            {
                Id = entity.PolicyId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,               
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId               
            };
            return dto;
        }

        protected override UmPolicy MapToEntity(PolicyDto dto)
        {
            var entity = new UmPolicy
            {
                PolicyId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,               
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy,                
            };
            return entity;
        }

        //public async Task<IEnumerable<PageDto>> GetAllPagesAsync()
        //{
        //    var entities = await _context.UmPages.ToListAsync();

        //    return entities.Select(s => new PageDto
        //    {
        //        Id = s.PageId,
        //        PageName = s.PageName,
        //        Description = s.Description
        //    }).ToList();
        //}

        //public async Task<IEnumerable<ControlDto>> GetAllControlsAsync()
        //{

        //    var entities = await _context.UmControls.ToListAsync();

        //    return entities.Select(s => new ControlDto
        //    {
        //        Id = s.ControlId,
        //        ControlName = s.ControlName,
        //        Description = s.Description
        //    }).ToList();
        //}
    }
}
