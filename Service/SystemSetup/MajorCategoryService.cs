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
    public class MajorCategoryService : BaseService<SsMajorCategory, MajorCategoryDto>, IMajorCategoryService
    {
        public MajorCategoryService(ISenProContext context) : base(context)
        {
        }

        public Task<List<AccountCodeDto>> GetAccountCodes()
        {
            return _context.SsAccountCodes.Where(x => x.IsActive).Select(p => new AccountCodeDto
            {
                Id = p.ItemTypeId,
                Code = p.Code,
                ItemTypeId = p.ItemTypeId,
                //ItemTypeName = p.ItemType?.Name,
                Description = p.Description
            }).ToListAsync();
        }

        protected override IQueryable<SsMajorCategory> IncludeNavigationProperties(IQueryable<SsMajorCategory> query)
        {
            return query.Include(o => o.AccountCode);
        }

        protected override IQueryable<SsMajorCategory> ApplySearchFilter(IQueryable<SsMajorCategory> query, string searchQuery)
        {
            return query.Where(p => new[] { p.Description, p.Code }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        protected override MajorCategoryDto MapToDto(SsMajorCategory entity)
        {
            var dto = new MajorCategoryDto
            {
                Id = entity.MajorCategoryId,
                Code = entity.Code,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                Name = entity.Name,
                AccountCodeId = entity.AccountCodeId,
                AccountCodeCode = entity.AccountCode?.Code,                
            };
            return dto;
        }

        protected override SsMajorCategory MapToEntity(MajorCategoryDto dto)
        {
            var entity = new SsMajorCategory
            {
                MajorCategoryId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy,
                AccountCodeId = dto.AccountCodeId,
                Name = dto.Name                
            };
            return entity;
        }
    }
}