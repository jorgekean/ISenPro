using EF;
using EF.Models;

using EF.Models.UserManagement;
using Service.Dto.SystemSetup;
using Service.Dto.UserManagement;
using Service.Service;
using Service.SystemSetup.Interface;
using Service.UserManagement.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.SystemSetup
{
    public class PurchasingTypeService : BaseService<SsPurchasingType, PurchasingTypeDto>, IPurchasingTypeService
    {
        public PurchasingTypeService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<SsPurchasingType> ApplySearchFilter(IQueryable<SsPurchasingType> query, string searchQuery)
        {
            return query.Where(p => new[] { p.Name, p.Code }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        protected override PurchasingTypeDto MapToDto(SsPurchasingType entity)
        {
            var dto = new PurchasingTypeDto
            {
                Id = entity.PurchasingTypeId,
                Code = entity.Code,
                Name = entity.Name,
                WithCondition = entity.WithCondition,
                Description = entity.Description,
                MaximumAmount = entity.MaximumAmount,
                MinimumAmount = entity.MinimumAmount,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId
            };
            return dto;
        }

        protected override SsPurchasingType MapToEntity(PurchasingTypeDto dto)
        {
            var entity = new SsPurchasingType
            {
                PurchasingTypeId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                MaximumAmount = dto.MaximumAmount,
                MinimumAmount = dto.MinimumAmount,
                WithCondition = dto.WithCondition,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy
            };
            return entity;
        }
    }
}
