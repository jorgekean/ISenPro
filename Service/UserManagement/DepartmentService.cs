using EF;
using EF.Models;

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
    public class DepartmentService : BaseService<UmDepartment, DepartmentDto>, IDepartmentService
    {
        public DepartmentService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<UmDepartment> IncludeNavigationProperties(IQueryable<UmDepartment> query)
        {
            return query.Include(o => o.Bureau).ThenInclude(i => i.Division);
        }

        protected override IQueryable<UmDepartment> ApplySearchFilter(IQueryable<UmDepartment> query, string searchQuery)
        {
            return query.Where(p => new[] { p.Name, p.Code, p.Description }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        public override IQueryable<UmDepartment> ApplyFilters(IQueryable<UmDepartment> query, List<Filter> filters)
        {
            if (filters != null && filters.Any())
            {
                // Apply each filter
                foreach (var filter in filters)
                {
                    if (filter.FilterOptions != null && filter.FilterOptions.Any())
                    {
                        // Apply filter using OR logic for FilterOptions
                        Expression<Func<UmDepartment, bool>> filterCondition = p => false; // Default false, will combine with OR

                        foreach (var option in filter.FilterOptions)
                        {
                            if (filter.FilterName.Equals("division", StringComparison.OrdinalIgnoreCase))
                            {
                                // Combine filter options with OR logic
                                var currentCondition = (Expression<Func<UmDepartment, bool>>)(p => p.Bureau != null && p.Bureau.Division != null && p.Bureau.Division.DivisionId != null && p.Bureau.Division.DivisionId == option.Value);
                                filterCondition = CombineWithOr(filterCondition, currentCondition);
                            }

                            if (filter.FilterName.Equals("bureau", StringComparison.OrdinalIgnoreCase))
                            {
                                // Combine filter options with OR logic
                                var currentCondition = (Expression<Func<UmDepartment, bool>>)(p => p.Bureau != null && p.Bureau.BureauId == option.Value);
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

        protected override DepartmentDto MapToDto(UmDepartment entity)
        {
            var dto = new DepartmentDto
            {
                Id = entity.DepartmentId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                BureauId = entity.BureauId,
                ResponsibilityCenter = entity.ResponsibilityCenter,
                Bureau = entity.Bureau == null ? null : new BureauDto
                {
                    Id = entity.Bureau.BureauId,
                    Code = entity.Bureau.Code,
                    Name = entity.Bureau.Name,
                    Description = entity.Bureau.Description,
                    IsActive = entity.Bureau.IsActive,
                    CreatedDate = entity.Bureau.CreatedDate,
                    CreatedBy = entity.Bureau.CreatedByUserId,
                    Division = entity.Bureau.Division == null ? null : new DivisionDto
                    {
                        Id = entity.Bureau.Division.DivisionId,
                        Code = entity.Bureau.Division.Code,
                        Name = entity.Bureau.Division.Name,
                        Description = entity.Bureau.Division.Description,
                        IsActive = entity.Bureau.Division.IsActive,
                        CreatedDate = entity.Bureau.Division.CreatedDate,
                        CreatedBy = entity.Bureau.Division.CreatedByUserId
                    }
                }

            };
            return dto;
        }

        protected override UmDepartment MapToEntity(DepartmentDto dto)
        {
            var entity = new UmDepartment
            {
                DepartmentId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy,
                BureauId = dto.BureauId,
                ResponsibilityCenter = dto.ResponsibilityCenter
            };
            return entity;
        }
    }
}
