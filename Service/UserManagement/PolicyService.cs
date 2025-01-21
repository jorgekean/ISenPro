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

        public override async Task<object> AddAsync(PolicyDto dto)
        {
            var entity = MapToEntity(dto);

            //// Track child entities generically
            //TrackChildEntities(entity);

            // Add the parent entity
            var result = await _dbSet.AddAsync(entity);

            // Save changes for parent
            await _context.SaveChangesAsync();

            // loop through dto.Roles
            dto.Roles.ForEach(role =>
            {
                var policyRole = new UmPolicyRole
                {
                    PolicyId = entity.PolicyId,
                    RoleId = role.RoleId,
                    CreatedByUserId = entity.CreatedByUserId,
                    CreatedDate = entity.CreatedDate,
                    IsActive = role.IsActive
                };
                _context.UmPolicyRoles.Add(policyRole);
            });


            // loop through dto.ModuleControls
            dto.ModuleControls.ForEach(role =>
            {
                var policyModuleControl = new UmPolicyModuleControl
                {
                    PolicyId = entity.PolicyId,
                    ModuleControlId = role.ModuleControlId,
                    IsChecked = role.IsChecked,
                    IsActive = role.IsActive,
                    CreatedByUserId = entity.CreatedByUserId,
                    CreatedDate = entity.CreatedDate
                };
                _context.UmPolicyModuleControls.Add(policyModuleControl);
            });         
            
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public override async Task UpdateAsync(PolicyDto dto)
        {
            var entity = MapToEntity(dto);           

            // update parent
            _dbSet.Update(entity);

            #region roles
            // update child roles: delete then insert
            _context.UmPolicyRoles.RemoveRange(_context.UmPolicyRoles.Where(x => x.PolicyId == entity.PolicyId));
            // loop through dto.Roles
            dto.Roles.ForEach(role =>
            {
                var policyRole = new UmPolicyRole
                {
                    PolicyId = entity.PolicyId,
                    RoleId = role.RoleId,
                    CreatedByUserId = entity.CreatedByUserId,
                    CreatedDate = entity.CreatedDate,
                    IsActive = role.IsActive
                };
                _context.UmPolicyRoles.Add(policyRole);
            });
            #endregion

            #region module controls
            // update child module controls: delete then insert
            _context.UmPolicyModuleControls.RemoveRange(_context.UmPolicyModuleControls.Where(x => x.PolicyId == entity.PolicyId));
            // loop through dto.ModuleControls
            dto.ModuleControls.ForEach(role =>
            {
                var policyModuleControl = new UmPolicyModuleControl
                {
                    PolicyId = entity.PolicyId,
                    ModuleControlId = role.ModuleControlId,
                    IsChecked = role.IsChecked,
                    IsActive = role.IsActive,
                    CreatedByUserId = entity.CreatedByUserId,
                    CreatedDate = entity.CreatedDate
                };
                _context.UmPolicyModuleControls.Add(policyModuleControl);
            });
            #endregion


            await _context.SaveChangesAsync();

            //
        }

        protected override PolicyDto MapToDto(UmPolicy entity)
        {
            var policyRoles = _context.UmPolicyRoles.Where(x => x.PolicyId == entity.PolicyId).ToList();
            var moduleControls = _context.UmPolicyModuleControls.Where(x => x.PolicyId == entity.PolicyId).ToList();

            var dto = new PolicyDto
            {
                Id = entity.PolicyId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,               
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,

                Roles = policyRoles.Select(s => new PolicyRoleDto
                {
                    RoleId = s.RoleId,
                    IsActive = s.IsActive,
                    PolicyId = s.PolicyId,
                    Id = s.PolicyRoleId,
                    CreatedBy = s.CreatedByUserId,
                }).ToList(),

                ModuleControls = moduleControls.Select(s => new PolicyModuleControlDto
                {
                    ModuleControlId = s.ModuleControlId,
                    IsChecked = s.IsChecked,
                    IsActive = s.IsActive,                    
                    PolicyId = s.PolicyId,
                    Id = s.PolicyModuleControlId,
                    CreatedBy = s.CreatedByUserId,
                }).ToList()

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
    }
}
