using EF;
using EF.Models;
using EF.Models.SystemSetup;
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
    public class ReferenceTableService : BaseService<SsReferenceTable, ReferenceTableDto>, IReferenceTableService
    {
        public ReferenceTableService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<SsReferenceTable> ApplySearchFilter(IQueryable<SsReferenceTable> query, string searchQuery)
        {
            return query.Where(p => p.Name.Contains(searchQuery) || p.Code.Contains(searchQuery) || p.Description.Contains(searchQuery));
        }

        protected override ReferenceTableDto MapToDto(SsReferenceTable entity)
        {
            var dto = new ReferenceTableDto
            {
                ReferenceTableId = entity.ReferenceTableId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive.GetValueOrDefault()
            };
            return dto;
        }

        protected override SsReferenceTable MapToEntity(ReferenceTableDto dto)
        {
            var entity = new SsReferenceTable
            {
                ReferenceTableId = dto.ReferenceTableId,
                Code = dto.Code,
                Name = dto.Name,
                IsActive = dto.IsActive,
                Description = dto.Description,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy
            };
            return entity;
        }
    }
}
