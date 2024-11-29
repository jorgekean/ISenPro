using EF;
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
    public class SubCategoryService : BaseService<SsSubCategory, SubCategoryDto>, ISubCategoryService
    {
        public SubCategoryService(ISenProContext context) : base(context)
        {
        }

        public Task<List<MajorCategoryDto>> GetMajorCategories()
        {
            return _context.SsMajorCategories.Where(x => x.IsActive).Include(s => s.AccountCode).Select(p => new MajorCategoryDto
            {
                Id = p.MajorCategoryId,
                Code = p.Code,
                Name = p.Name,
                AccountCodeCode = p.AccountCode != null ? p.AccountCode.Code : "",
                //ItemTypeName = p.ItemType?.Name,
                Description = p.Description
            }).ToListAsync();
        }

        protected override IQueryable<SsSubCategory> IncludeNavigationProperties(IQueryable<SsSubCategory> query)
        {
            return query.Include(o => o.MajorCategory).ThenInclude(s => s.AccountCode).ThenInclude(s => s.ItemType);
        }

        protected override IQueryable<SsSubCategory> ApplySearchFilter(IQueryable<SsSubCategory> query, string searchQuery)
        {
            return query.Where(p => new[] { p.Description, p.Code, p.Name }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        protected override SubCategoryDto MapToDto(SsSubCategory entity)
        {
            var dto = new SubCategoryDto
            {
                Id = entity.SubCategoryId,
                Code = entity.Code,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                Name = entity.Name,
                MajorCategoryCode = entity.MajorCategory?.Code,
                MajorCategoryId = entity.MajorCategoryId,
                MajorCategoryName = entity.MajorCategory?.Name,
                AccountCode = entity.MajorCategory?.AccountCode?.Code,
                ItemTypeName = entity.MajorCategory?.AccountCode?.ItemType?.Name
            };
            return dto;
        }

        protected override SsSubCategory MapToEntity(SubCategoryDto dto)
        {
            var entity = new SsSubCategory
            {
                SubCategoryId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy,
                MajorCategoryId = dto.MajorCategoryId,
                Name = dto.Name
            };
            return entity;
        }
    }
}
