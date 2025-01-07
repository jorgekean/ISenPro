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
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service.SystemSetup
{
    public class SupplierService : BaseService<SsSupplier, SupplierDto>, ISupplierService
    {
        public SupplierService(ISenProContext context) : base(context)
        {
        }

        public Task<List<ReferenceTableDto>> GetIndustries()
        {
            return _context.SsReferenceTables.Where(x => x.IsActive == true && x.RefTableId == 3).Select(p => new ReferenceTableDto
            {
                ReferenceTableId = p.ReferenceTableId,
                Name = p.Name
            }).OrderBy(x => x.Name).ToListAsync();
        }

        protected override IQueryable<SsSupplier> ApplySearchFilter(IQueryable<SsSupplier> query, string searchQuery)
        {
            return query.Where(p => new[] { p.CompanyName, p.Address }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        protected override SupplierDto MapToDto(SsSupplier entity)
        {
            var industryName = entity.Industry.HasValue
                                    ? _context.SsReferenceTables.FirstOrDefault(x => x.ReferenceTableId == entity.Industry.Value).Name
                                    : string.Empty;

            var dto = new SupplierDto
            {
                Id = entity.SupplierId,
                CompanyName = entity.CompanyName ?? string.Empty,
                Blacklist = entity.Blacklist,
                Address = entity.Address ?? string.Empty,
                EmailAddress = entity.EmailAddress ?? string.Empty,
                Remarks = entity.Remarks ?? string.Empty,
                FaxNumber = entity.FaxNumber ?? string.Empty,
                Tin = entity.Tin ?? string.Empty,
                Industry = entity.Industry ?? 0,
                IndustryName = industryName,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = (int)entity.CreatedByUserId
            };
            return dto;
        }

        protected override SsSupplier MapToEntity(SupplierDto dto)
        {
            var entity = _dbSet.Find(dto.Id) ?? new SsSupplier();

            entity.CompanyName = dto.CompanyName;
            entity.Blacklist = dto.Blacklist;
            entity.Address = dto.Address;
            entity.EmailAddress = dto.EmailAddress;
            entity.Remarks = dto.Remarks;
            entity.FaxNumber = dto.FaxNumber;
            entity.Tin = dto.Tin;
            entity.Industry = dto.Industry;

            if (dto.Id == 0)
            {
                entity.IsActive = true;
                entity.CreatedDate = DateTime.Now;
                entity.CreatedByUserId = 1;
            }

            return entity;
        }
    }
}
