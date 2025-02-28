using EF;
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
    public class PSDBMCatalogueService : BaseService<SsPsdbmcatalogue, PSDBMCatalogueDto>, IPSDBMCatalogueService
    {
        private readonly IGenericCacheService _cacheService;
        public PSDBMCatalogueService(ISenProContext context,
                                     IGenericCacheService cacheService
            ) : base(context)
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

        public override IQueryable<SsPsdbmcatalogue> ApplyFilters(IQueryable<SsPsdbmcatalogue> query, List<Filter> filters)
        {
            if (filters != null && filters.Any())
            {
                // Apply each filter
                foreach (var filter in filters)
                {
                    if (filter.FilterOptions != null && filter.FilterOptions.Any())
                    {
                        // Apply filter using OR logic for FilterOptions
                        Expression<Func<SsPsdbmcatalogue, bool>> filterCondition = p => false; // Default false, will combine with OR

                        foreach (var option in filter.FilterOptions)
                        {
                            if (filter.FilterName.ToLower() == "unitofmeasurement")
                            {
                                // Combine filter options with OR logic
                                var currentCondition = (Expression<Func<SsPsdbmcatalogue, bool>>)(p => p.UnitOfMeasurementId == option.Value);
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

        protected override IQueryable<SsPsdbmcatalogue> IncludeNavigationProperties(IQueryable<SsPsdbmcatalogue> query)
        {
            return query.Include(o => o.UnitOfMeasurement).Include(o => o.ItemType).Include(o => o.AccountCode).Include(o => o.MajorCategory).Include(o => o.SubCategory);
        }

        protected override IQueryable<SsPsdbmcatalogue> ApplySearchFilter(IQueryable<SsPsdbmcatalogue> query, string searchQuery)
        {
            // for efficiency, if there is no search query, return the query as is
            if (string.IsNullOrEmpty(searchQuery))
            {
                return query;

            }

            return query.Where(p => new[] { p.Description, 
                                            p.Code, 
                                            p.Description,
                                            p.UnitOfMeasurement != null ? p.UnitOfMeasurement.Code : string.Empty,
                                            p.ItemType != null ? p.ItemType.Name : string.Empty,
                                          }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        protected override PSDBMCatalogueDto MapToDto(SsPsdbmcatalogue entity)
        {
            var dto = new PSDBMCatalogueDto
            {
                Id = entity.PsdbmcatalogueId,
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

        protected override SsPsdbmcatalogue MapToEntity(PSDBMCatalogueDto dto)
        {
            var entity = _dbSet.Find(dto.Id) ?? new SsPsdbmcatalogue();

            entity.Code = dto.Code;
            entity.Description = dto.Description;
            entity.ItemTypeId = dto.ItemTypeId;
            entity.AccountCodeId = dto.AccountCodeId;
            entity.MajorCategoryId = dto.MajorCategoryId;
            entity.SubCategoryId = dto.SubCategoryId;
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
        public async Task<List<PSDBMCatalogueDto>> GetAllCurrent()
        {
            var query = IncludeNavigationProperties(_dbSet);
            var entities = await query.Where(e => e.IsActive && e.IsCurrent.HasValue && e.IsCurrent.Value).ToListAsync();
            return entities.Select(MapToDto).ToList();
        }

        #region overriden because of caching
        public override Task<object> AddAsync(PSDBMCatalogueDto dto)
        {
            var result = base.AddAsync(dto);

            // clear cached items
            _cacheService.Remove("CachedPSDBMCatalogue");

            return result;
        }

        public override Task UpdateAsync(PSDBMCatalogueDto dto)
        {
            var result =  base.UpdateAsync(dto);

            // clear cached items
            _cacheService.Remove("CachedPSDBMCatalogue");

            return result;
        }

        public override Task DeleteAsync(int id)
        {
            var result = base.DeleteAsync(id);

            // clear cached items
            _cacheService.Remove("CachedPSDBMCatalogue");

            return result;
        }
        #endregion
    }
}
