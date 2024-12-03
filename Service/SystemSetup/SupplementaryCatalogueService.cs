using EF;
using EF.Models;
using EF.Models.SystemSetup;
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
    public class SupplementaryCatalogueService : BaseService<SsSupplementaryCatalogue, SupplementaryCatalogueDto>, ISupplementaryCatalogueService
    {
        public SupplementaryCatalogueService(ISenProContext context) : base(context)
        {
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
            return query.Include(o => o.UnitOfMeasurement);
        }

        protected override IQueryable<SsSupplementaryCatalogue> ApplySearchFilter(IQueryable<SsSupplementaryCatalogue> query, string searchQuery)
        {
            // for efficiency, if there is no search query, return the query as is
            if (string.IsNullOrEmpty(searchQuery))
            {
                return query;

            }

            return query.Where(p => new[] { p.Description, 
                                            p.Code, 
                                            p.Description,
                                            p.UnitOfMeasurement.Code,
                                            p.UnitOfMeasurement.Name
                                          }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        protected override SupplementaryCatalogueDto MapToDto(SsSupplementaryCatalogue entity)
        {
            var dto = new SupplementaryCatalogueDto
            {
                Id = entity.SupplementaryCatalogueId,
                Code = entity.Code,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                UnitOfMeasurementId = entity.UnitOfMeasurementId,
                UnitOfMeasurementCode = entity.UnitOfMeasurement.Code
            };
            return dto;
        }

        protected override SsSupplementaryCatalogue MapToEntity(SupplementaryCatalogueDto dto)
        {
            var entity = _dbSet.Find(dto.Id) ?? new SsSupplementaryCatalogue();

            entity.Code = dto.Code;
            entity.Description = dto.Description;
            entity.UnitOfMeasurementId = dto.UnitOfMeasurementId;
            
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
