using EF;
using EF.Models;
using EF.Models.SystemSetup;
using EF.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Service.Dto.UserManagement;
using Service.Service;
using Service.UserManagement.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.UserManagement
{
    public class BureauService : BaseService<UmBureau, BureauDto>, IBureauService
    {
        public BureauService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<UmBureau> IncludeNavigationProperties(IQueryable<UmBureau> query)
        {
            return query.Include(o => o.Division);
        }

        public override IQueryable<UmBureau> ApplyFilters(IQueryable<UmBureau> query, List<Filter> filters)
        {
            if (filters != null && filters.Any())
            {
                // Apply each filter
                foreach (var filter in filters)
                {
                    if (filter.FilterOptions != null && filter.FilterOptions.Any())
                    {
                        // Apply filter using OR logic for FilterOptions
                        Expression<Func<UmBureau, bool>> filterCondition = p => false; // Default false, will combine with OR

                        foreach (var option in filter.FilterOptions)
                        {
                            if (filter.FilterName.ToLower() == "division")
                            {
                                // Combine filter options with OR logic
                                var currentCondition = (Expression<Func<UmBureau, bool>>)(p => p.DivisionId == option.Value);
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

        protected override IQueryable<UmBureau> ApplySearchFilter(IQueryable<UmBureau> query, string searchQuery)
        {
            return query.Where(p => p.Name.Contains(searchQuery) || p.Code.Contains(searchQuery) || (string.IsNullOrWhiteSpace(p.Description) || p.Description.Contains(searchQuery)));
        }

        protected override BureauDto MapToDto(UmBureau entity)
        {
            var dto = new BureauDto
            {
                Id = entity.BureauId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                DivisionId = entity.DivisionId,
                GroupId = entity.GroupId,
                Division = entity.Division == null ? null : new DivisionDto
                {
                    Id = entity.Division.DivisionId,
                    Code = entity.Division.Code,
                    Name = entity.Division.Name,
                    Description = entity.Division.Description,
                    IsActive = entity.Division.IsActive,
                    CreatedDate = entity.Division.CreatedDate,
                    CreatedBy = entity.Division.CreatedByUserId
                }                
            };
            return dto;
        }

        protected override UmBureau MapToEntity(BureauDto dto)
        {
            var entity = new UmBureau
            {
                BureauId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy,
                DivisionId = dto.DivisionId.GetValueOrDefault(),
                GroupId = dto.GroupId.GetValueOrDefault(),               
            };
            return entity;
        }
    }
}
