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
                Id = p.ReferenceTableId,
                Name = p.Name
            }).OrderBy(x => x.Name).ToListAsync();
        }

        protected override IQueryable<SsSupplier> ApplySearchFilter(IQueryable<SsSupplier> query, string searchQuery)
        {
            return query.Where(p => new[] { p.CompanyName, p.Address }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        public override async Task<object> AddAsync(SupplierDto dto)
        {
            var entity = MapToEntity(dto);

            // Add the parent entity
            var result = await _dbSet.AddAsync(entity);

            // Save changes for parent
            await _context.SaveChangesAsync();

            #region Contact People

            // loop through dto.WorkStepApprovers
            dto.SupplierContactPeople.ForEach(item =>
            {
                var contactPerson = new SsSupplierContactPerson
                {
                    SupplierId = entity.SupplierId,
                    ContactNumber = item.ContactNumber,
                    ContactPerson = item.ContactPerson,
                    CreatedByUserId = entity.CreatedByUserId,
                    CreatedDate = entity.CreatedDate,
                    IsActive = item.IsActive
                };
                _context.SsSupplierContactPeople.Add(contactPerson);
            });

            await _context.SaveChangesAsync();

            #endregion

            return result.Entity;
        }

        public override async Task<object> UpdateAsync(SupplierDto dto)
        {
            var entity = MapToEntity(dto);

            // Add the parent entity
            var result = _dbSet.Update(entity);

            // update child roles: delete then insert
            _context.SsSupplierContactPeople.RemoveRange(_context.SsSupplierContactPeople.Where(x => x.SupplierId == entity.SupplierId));

            #region Contact People

            // loop through dto.WorkStepApprovers
            dto.SupplierContactPeople.ForEach(item =>
            {
                var contactPerson = new SsSupplierContactPerson
                {
                    SupplierId = entity.SupplierId,
                    ContactNumber = item.ContactNumber,
                    ContactPerson = item.ContactPerson,
                    CreatedByUserId = entity.CreatedByUserId,
                    CreatedDate = entity.CreatedDate,
                    IsActive = item.IsActive
                };
                _context.SsSupplierContactPeople.Add(contactPerson);
            });

            await _context.SaveChangesAsync();

            #endregion

            return result.Entity;
        }

        public override async Task<SupplierDto> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);

                await _context.Entry(entity).Collection(x => x.SsSupplierContactPeople).LoadAsync();

                if (entity != null)
                {
                    return MapToDto(entity);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return null;
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
                CreatedBy = (int)entity.CreatedByUserId,
                SupplierContactPeople = entity.SsSupplierContactPeople.Where(x => x.IsActive == true).Select(x => new SupplierContactPersonDto
                {
                    Id = x.SupplierContactPersonId,
                    TempGuId = Guid.NewGuid().ToString(),
                    SupplierId = x.SupplierId,
                    ContactPerson = x.ContactPerson,
                    ContactNumber = x.ContactNumber
                }).ToList()
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
