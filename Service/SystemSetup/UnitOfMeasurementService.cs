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
    public class UnitOfMeasurementService : BaseService<SsUnitOfMeasurement, UnitOfMeasurementDto>, IUnitOfMeasurementService
    {
        public UnitOfMeasurementService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<SsUnitOfMeasurement> ApplySearchFilter(IQueryable<SsUnitOfMeasurement> query, string searchQuery)
        {
            return query.Where(p => p.Name.Contains(searchQuery) || p.Code.Contains(searchQuery));
        }

        protected override UnitOfMeasurementDto MapToDto(SsUnitOfMeasurement entity)
        {
            var dto = new UnitOfMeasurementDto
            {
                Id = entity.UnitOfMeasurementId,
                Code = entity.Code,
                Name = entity.Name,
                IsActive = entity.IsActive.GetValueOrDefault(),
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId
            };
            return dto;
        }

        protected override SsUnitOfMeasurement MapToEntity(UnitOfMeasurementDto dto)
        {
            var entity = new SsUnitOfMeasurement
            {
                UnitOfMeasurementId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy
            };
            return entity;
        }
    }
}
