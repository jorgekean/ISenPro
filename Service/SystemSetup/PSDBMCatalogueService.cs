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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            return query.Include(o => o.UnitOfMeasurement).Include(o => o.ItemType).Include(o => o.AccountCode).Include(o => o.MajorCategory).Include(o => o.SubCategory).Include(o => o.SsPsdbmcatalogueOffices);
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

        public override async Task<(IEnumerable<PSDBMCatalogueDto> Data, int TotalRecords)> GetPagedAndFilteredAsync(PagingParameters pagingParameters)
        {
            var query = _context.SsPsdbmcatalogues.Where(x => x.IsActive && x.IsCurrent == true);

            if (pagingParameters.Filters != null && pagingParameters.Filters.Any())
            {
                // Apply dynamic filters
                query = ApplyFilters(query, pagingParameters.Filters);
            }

            if (!string.IsNullOrEmpty(pagingParameters.SearchQuery))
            {
                query = ApplySearchFilter(query, pagingParameters.SearchQuery);
            }

            var totalRecords = await query.CountAsync();
            var data = await query.Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                                  .Take(pagingParameters.PageSize)
                                  .ToListAsync();

            var dtoData = data.Select(MapToDto).ToList();
            return (dtoData, totalRecords);
        }

        public override async Task<PSDBMCatalogueDto> GetByIdAsync(int id)
        {
            var entity = await _context.SsPsdbmcatalogues.Include(o => o.UnitOfMeasurement).Include(o => o.ItemType).Include(o => o.AccountCode).Include(o => o.MajorCategory).Include(o => o.SubCategory).Include(o => o.SsPsdbmcatalogueOffices).ThenInclude(x => x.Department).ThenInclude(x => x.Bureau).FirstOrDefaultAsync(x => x.PsdbmcatalogueId == id);

            return entity != null ? MapToDto(entity) : null;
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
                CatalogueYearStr = entity.CatalogueYear.HasValue ? entity.CatalogueYear.Value.Year.ToString() : string.Empty,
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
                ItemTypeId = entity.ItemTypeId,
                CatalogueOffices = entity.SsPsdbmcatalogueOffices.Where(x => x.IsActive).Select(x => new PSDBMCatalogueOfficeDto
                {
                    PSDBMCatalogueId = x.PsdbmcatalogueId,
                    DepartmentId = x.DepartmentId,
                    Department = new DepartmentDto
                    {
                        Bureau = new BureauDto
                        {
                            Name = x.Department.Bureau.Name
                        },
                        Name = x.Department.Name
                    }
                }).ToList()
            };
            return dto;
        }

        public override async Task UpdateAsync(PSDBMCatalogueDto dto)
        {
            var entity = MapToEntity(dto);

            // update parent
            _dbSet.Update(entity);

            #region CatalogueOffices

            // update child roles: delete then insert
            _context.SsPsdbmcatalogueOffices.RemoveRange(_context.SsPsdbmcatalogueOffices.Where(x => x.PsdbmcatalogueId == entity.PsdbmcatalogueId));

            // loop through dto.WorkStepApprovers
            dto.CatalogueOffices.ForEach(item =>
            {
                var catalogueOffice = new SsPsdbmcatalogueOffice
                {
                    DepartmentId = item.DepartmentId.Value,
                    PsdbmcatalogueId = entity.PsdbmcatalogueId,
                    CreatedByUserId = entity.CreatedByUserId,
                    CreatedDate = entity.CreatedDate,
                    IsActive = item.IsActive
                };
                _context.SsPsdbmcatalogueOffices.Add(catalogueOffice);
            });

            await _context.SaveChangesAsync();

            #endregion
        }

        protected override SsPsdbmcatalogue MapToEntity(PSDBMCatalogueDto dto)
        {
            var entity = _dbSet.Find(dto.Id) ?? new SsPsdbmcatalogue();

            var catalogueYear = new DateTime(DateTime.Now.Year, 01, 01);

            if (!string.IsNullOrEmpty(dto.CatalogueYearStr))
            {
                catalogueYear = new DateTime(Convert.ToInt32(dto.CatalogueYearStr), 01, 01);
            }

            entity.Code = dto.Code;
            entity.Description = dto.Description;
            entity.ItemTypeId = dto.ItemTypeId;
            entity.AccountCodeId = dto.AccountCodeId;
            entity.MajorCategoryId = dto.MajorCategoryId;
            entity.SubCategoryId = dto.SubCategoryId == 0 || dto.SubCategoryId == null ? null : dto.SubCategoryId.Value;
            entity.UnitOfMeasurementId = dto.UnitOfMeasurementId;
            entity.UnitPrice = dto.UnitPrice;
            entity.CatalogueYear = catalogueYear;
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
        public async Task<List<PSDBMCatalogueDto>> GetAllCurrent(int year)
        {
            var query = IncludeNavigationProperties(_dbSet);

            var entities = await query.Where(e => e.IsActive
            && e.CatalogueYear.HasValue && e.CatalogueYear.Value.Year == year
            && e.IsCurrent.HasValue && e.IsCurrent.Value).ToListAsync();

            return entities.Select(MapToDto).ToList();
        }

        #region overriden because of caching
        public override Task<object> AddAsync(PSDBMCatalogueDto dto)
        {
            var result = base.AddAsync(dto);

            // clear cached items
            _cacheService.Remove($"CachedPSDBMCatalogue_{dto.CatalogueYear?.Year}");

            return result;
        }

        public override Task DeleteAsync(int id)
        {
            var result = base.DeleteAsync(id);

            // clear cached items
            _cacheService.Remove($"CachedPSDBMCatalogue_{DateTime.Now.Year}");// to do: get year from db

            return result;
        }
        #endregion
    }
}
