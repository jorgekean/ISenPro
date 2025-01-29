using EF;
using EF.Models;
using EF.Models.SystemSetup;
using EF.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Service.Dto.SystemSetup;
using Service.Dto.UserManagement;
using Service.Enums;
using Service.Helpers;
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

        public Task<List<ReferenceTableDto>> GetListOfReference(int refId)
        {
            return _context.SsReferenceTables.Where(x => x.IsActive == true && x.RefTableId == refId).Select(p => new ReferenceTableDto
            {
                Id = p.ReferenceTableId,
                Name = p.Name
            }).OrderBy(x => x.Name).ToListAsync();
        }

        protected override ReferenceTableDto MapToDto(SsReferenceTable entity)
        {
            var dto = new ReferenceTableDto
            {
                Id = entity.ReferenceTableId,
                RefTableId = entity.RefTableId,
                RefTableName = entity.RefTableId.HasValue ? EnumHelper.GetEnumByValue<Enums.ReferenceTableModule>(entity.RefTableId.Value)?.ToString() : string.Empty, 
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
                ReferenceTableId = dto.Id.GetValueOrDefault(),
                RefTableId = dto.RefTableId,
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedByUserId = 1,
            };
                        
            return entity;
        }
    }
}
