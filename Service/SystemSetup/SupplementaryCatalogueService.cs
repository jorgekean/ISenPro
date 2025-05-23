﻿using EF;
using EF.Models;

using EF.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Service.Cache;
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
    public class SupplementaryCatalogueService : BaseService<SsSupplementaryCatalogue, SupplementaryCatalogueDto>, ISupplementaryCatalogueService
    {
        private readonly IGenericCacheService _cacheService;
        public SupplementaryCatalogueService(ISenProContext context,
            IGenericCacheService cacheService) : base(context)
        {
            _cacheService = cacheService;
        }

        public Task<List<UnitOfMeasurementDto>> GetUnitOfMeasurements()
        {
            return _context.SsUnitOfMeasurements.Where(x => x.IsActive == true).Select(p => new UnitOfMeasurementDto
            {
                Id = p.UnitOfMeasurementId,
                Code = p.Code,
                Name = p.Name                
            }).ToListAsync();
        }

        public Task<List<AccountCodeDto>> GetAccountCodes()
        {
            return _context.SsAccountCodes.Where(x => x.IsActive == true).Select(p => new AccountCodeDto
            {
                Id = p.AccountCodeId,
                Code = p.Code + " - " + p.Description,
                Description = p.Code + " - " + p.Description,
            }).ToListAsync();
        }
        public override IQueryable<SsSupplementaryCatalogue> ApplyFilters(IQueryable<SsSupplementaryCatalogue> query, List<Filter> filters)
        {
            if (filters != null && filters.Any())
            {
                // Apply each filter
                foreach (var filter in filters)
                {
                    if (filter.FilterOptions != null && filter.FilterOptions.Any())
                    {
                        // Apply filter using OR logic for FilterOptions
                        Expression<Func<SsSupplementaryCatalogue, bool>> filterCondition = p => false; // Default false, will combine with OR

                        foreach (var option in filter.FilterOptions)
                        {
                            if (filter.FilterName.ToLower() == "unitofmeasurement")
                            {
                                // Combine filter options with OR logic
                                var currentCondition = (Expression<Func<SsSupplementaryCatalogue, bool>>)(p => p.UnitOfMeasurementId == option.Value);
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

        protected override IQueryable<SsSupplementaryCatalogue> IncludeNavigationProperties(IQueryable<SsSupplementaryCatalogue> query)
        {
            return query.Include(o => o.UnitOfMeasurement).Include(o => o.ItemType).Include(o => o.AccountCode).Include(o => o.MajorCategory).Include(o => o.SubCategory);
        }

        protected override IQueryable<T> ApplySearchFilter<T>(IQueryable<T> query, string searchQuery)
        {
            // If the type is MyEntity, cast the query and apply filtering.
            if (typeof(T) == typeof(VSupplementaryCatalogueIndex))
            {
                var typedQuery = query as IQueryable<VSupplementaryCatalogueIndex>;
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    typedQuery = typedQuery.Where(p => new[] { p.Code, p.Description, p.UnitOfMeasurementCode, p.ItemTypeName, p.MajorCategoryName, p.SubCategoryName }
                             .Any(value => value != null && value.Contains(searchQuery)));
                }

                // Cast back to IQueryable<T> and return
                return typedQuery as IQueryable<T>;
            }

            // Otherwise, for any other type, you can either return the unfiltered query or add your own logic.
            return query;
        }

        //protected override IQueryable<SsSupplementaryCatalogue> ApplySearchFilter(IQueryable<SsSupplementaryCatalogue> query, string searchQuery)
        //{
        //    // for efficiency, if there is no search query, return the query as is
        //    if (string.IsNullOrEmpty(searchQuery))
        //    {
        //        return query;

        //    }

        //    return query.Where(p => new[] { p.Description, 
        //                                    p.Code, 
        //                                    p.Description,
        //                                    p.UnitOfMeasurement != null ? p.UnitOfMeasurement.Code : string.Empty,
        //                                    p.ItemType != null ? p.ItemType.Name : string.Empty,
        //                                  }
        //                    .Any(value => value != null && value.Contains(searchQuery)));
        //}

        protected override SupplementaryCatalogueDto MapToDto(SsSupplementaryCatalogue entity)
        {
            var dto = new SupplementaryCatalogueDto
            {
                Id = entity.SupplementaryCatalogueId,
                Code = entity.Code,
                Description = entity.Description,
                Remarks = entity.Remarks,
                UnitPrice = entity.UnitPrice.HasValue ? entity.UnitPrice.Value : 0,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                UnitOfMeasurementId = entity.UnitOfMeasurementId,
                UnitOfMeasurementCode = entity.UnitOfMeasurement?.Code,
                AccountCodeId = entity.AccountCodeId,
                AccountCodeDescription = entity.AccountCode?.Description,
                MajorCategoryId = entity.MajorCategoryId,
                MajorCategoryName = entity.MajorCategory?.Name,
                SubCategoryName = entity.SubCategory?.Name,
                SubCategoryId = entity.SubCategoryId,
                ItemTypeName = entity.ItemType?.Name,
                ItemTypeId = entity.ItemTypeId
            };
            return dto;
        }

        protected override SsSupplementaryCatalogue MapToEntity(SupplementaryCatalogueDto dto)
        {
            var entity = _dbSet.Find(dto.Id) ?? new SsSupplementaryCatalogue();

            entity.Code = dto.Code;
            entity.Description = dto.Description;
            entity.ItemTypeId = dto.ItemTypeId;
            entity.AccountCodeId = dto.AccountCodeId;
            entity.MajorCategoryId = dto.MajorCategoryId;
            entity.SubCategoryId = dto.SubCategoryId == 0 || dto.SubCategoryId == null ? null : dto.SubCategoryId.Value;
            entity.UnitOfMeasurementId = dto.UnitOfMeasurementId;
            entity.UnitPrice = dto.UnitPrice;
            entity.Remarks = dto.Remarks;

            if (dto.Id == 0)
            {
                entity.IsActive = true;
                entity.CreatedDate = DateTime.Now;
                entity.CreatedByUserId = 1;
            }

            return entity;
        }

        /// <summary>
        /// Get Current Catalogue
        /// </summary>
        /// <returns></returns>
        public async Task<List<SupplementaryCatalogueDto>> GetAllCurrent(int year)
        {
            var query = IncludeNavigationProperties(_dbSet);

            var entities = await query.Where(e => e.IsActive 
            && e.CatalogueYear.HasValue && e.CatalogueYear.Value.Year == year
            && e.IsCurrent.HasValue && e.IsCurrent.Value).ToListAsync();

            return entities.Select(MapToDto).ToList();
        }

        #region overriden because of caching
        public override Task<object> AddAsync(SupplementaryCatalogueDto dto)
        {
            var result = base.AddAsync(dto);

            // clear cached items
            _cacheService.Remove("CachedSupplementaryCatalogue");

            return result;
        }

        public override Task UpdateAsync(SupplementaryCatalogueDto dto)
        {
            var result = base.UpdateAsync(dto);

            // clear cached items
            _cacheService.Remove("CachedSupplementaryCatalogue");

            return result;
        }

        public override Task DeleteAsync(int id)
        {
            var result = base.DeleteAsync(id);

            // clear cached items
            _cacheService.Remove("CachedSupplementaryCatalogue");

            return result;
        }
        #endregion
    }
}
