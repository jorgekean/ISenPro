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
using System.Linq.Expressions;
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

        public override IQueryable<SsSubCategory> ApplyFilters(IQueryable<SsSubCategory> query, List<Filter> filters)
        {
            if (filters != null && filters.Any())
            {
                // Apply each filter
                foreach (var filter in filters)
                {
                    if (filter.FilterOptions != null && filter.FilterOptions.Any())
                    {
                        // Apply filter using OR logic for FilterOptions
                        Expression<Func<SsSubCategory, bool>> filterCondition = p => false; // Default false, will combine with OR

                        foreach (var option in filter.FilterOptions)
                        {
                            if (filter.FilterName.Equals("accountcode", StringComparison.OrdinalIgnoreCase))
                            {
                                // Combine filter options with OR logic
                                var currentCondition = (Expression<Func<SsSubCategory, bool>>)(p => p.MajorCategory != null && p.MajorCategory.AccountCode != null && p.MajorCategory.AccountCode.AccountCodeId == option.Value);
                                filterCondition = CombineWithOr(filterCondition, currentCondition);
                            }

                            if (filter.FilterName.Equals("majorcategory", StringComparison.OrdinalIgnoreCase))
                            {
                                // Combine filter options with OR logic
                                var currentCondition = (Expression<Func<SsSubCategory, bool>>)(p => p.MajorCategory != null && p.MajorCategory.MajorCategoryId == option.Value);
                                filterCondition = CombineWithOr(filterCondition, currentCondition);
                            }
                        }

                        // Apply the OR condition to the query
                        query = query.Where(filterCondition);
                    }
                }
            }

            return query;
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
