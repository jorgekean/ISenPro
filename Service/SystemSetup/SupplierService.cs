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

namespace Service.SystemSetup
{
    public class SupplierService : BaseService<SsSupplier, SupplierDto>, ISupplierService
    {
        public SupplierService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<SsSupplier> ApplySearchFilter(IQueryable<SsSupplier> query, string searchQuery)
        {
            return query.Where(p => new[] { p.CompanyName, p.Address }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        protected override SupplierDto MapToDto(SsSupplier entity)
        {
            var dto = new SupplierDto
            {
                Id = entity.SupplierId,
                CompanyName = entity.CompanyName ?? string.Empty,
                Blacklist = entity.Blacklist ?? false,
                Address = entity.Address ?? string.Empty,
                EmailAddress = entity.EmailAddress ?? string.Empty,
                Remarks = entity.Remarks ?? string.Empty,
                FaxNumber = entity.FaxNumber ?? string.Empty,
                Tin = entity.Tin ?? string.Empty,
                Industry = entity.Industry ?? 0,
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

            if (dto.Id == 0)
            {
                entity.IsActive = true;
                entity.CreatedDate = DateTime.Now;
                entity.CreatedByUserId = 1;
            }
            else
            {
                entity.UpdatedDate = DateTime.Now;
                entity.UpdatedByUserId = 1;
            }            

            return entity;
        }
    }
}
