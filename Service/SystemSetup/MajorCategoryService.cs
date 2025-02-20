using EF;
using EF.Models;

using EF.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Service.Dto.SystemSetup;
using Service.Dto.UserManagement;
using Service.Service;
using Service.SystemSetup.Interface;
using Service.UserManagement.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
                Id = p.AccountCodeId,
                Code = p.Code,
                ItemTypeId = p.ItemTypeId,
                //ItemTypeName = p.ItemType?.Name,
                Description = p.Description
            }).ToListAsync();
        }

        public override IQueryable<SsMajorCategory> ApplyFilters(IQueryable<SsMajorCategory> query, List<Filter> filters)
        {
            if (filters != null && filters.Any())
            {
                // Apply each filter
                foreach (var filter in filters)
                {
                    if (filter.FilterOptions != null && filter.FilterOptions.Any())
                    {
                        // Apply filter using OR logic for FilterOptions
                        Expression<Func<SsMajorCategory, bool>> filterCondition = p => false; // Default false, will combine with OR

                        foreach (var option in filter.FilterOptions)
                        {
                            if (filter.FilterName.ToLower() == "accountcode")
                            {
                                // Combine filter options with OR logic
                                var currentCondition = (Expression<Func<SsMajorCategory, bool>>)(p => p.AccountCodeId == option.Value);
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

        protected override IQueryable<SsMajorCategory> IncludeNavigationProperties(IQueryable<SsMajorCategory> query)
        {
            return query.Include(o => o.AccountCode).ThenInclude(i => i.ItemType);
        }

        protected override IQueryable<SsMajorCategory> ApplySearchFilter(IQueryable<SsMajorCategory> query, string searchQuery)
        {
            // for efficiency, if there is no search query, return the query as is
            if (string.IsNullOrEmpty(searchQuery))
            {
                return query;

            }

            return query.Where(p => new[] { p.Description, p.Code, p.Name,
                                            p.AccountCode != null ? p.AccountCode.Code : null,
                                            p.AccountCode != null ? p.AccountCode.Description : null,
                                            p.AccountCode != null && p.AccountCode.ItemType != null ? p.AccountCode.ItemType.Name : null }
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
                ItemTypeName = entity.AccountCode?.ItemType?.Name
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
