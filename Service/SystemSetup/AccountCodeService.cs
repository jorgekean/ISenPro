﻿using EF;
using EF.Models;
using EF.Models.SystemSetup;
using EF.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
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
    public class AccountCodeService : BaseService<SsAccountCode, AccountCodeDto>, IAccountCodeService
    {
        public AccountCodeService(ISenProContext context) : base(context)
        {
        }

        public Task<List<ItemTypeDto>> GetItemTypes()
        {
            return _context.SsItemTypes.Where(x => x.IsActive).Select(p => new ItemTypeDto
            {
                Id = p.ItemTypeId,
                Name = p.Name,
                Description = p.Description
            }).ToListAsync();
        }

        protected override IQueryable<SsAccountCode> IncludeNavigationProperties(IQueryable<SsAccountCode> query)
        {
            return query.Include(o => o.ItemType);
        }

        protected override IQueryable<SsAccountCode> ApplySearchFilter(IQueryable<SsAccountCode> query, string searchQuery)
        {
            return query.Where(p => new[] { p.Description, p.Code }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        protected override AccountCodeDto MapToDto(SsAccountCode entity)
        {
            var dto = new AccountCodeDto
            {
                Id = entity.AccountCodeId,
                Code = entity.Code,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                ItemTypeId = entity.ItemTypeId,
                ItemTypeName = entity.ItemType?.Name
            };
            return dto;
        }

        protected override SsAccountCode MapToEntity(AccountCodeDto dto)
        {
            var entity = new SsAccountCode
            {
                AccountCodeId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy,
                ItemTypeId = dto.ItemTypeId
            };
            return entity;
        }
    }
}