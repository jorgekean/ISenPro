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
    public class SectionService : BaseService<UmSection, SectionDto>, ISectionService
    {
        public SectionService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<UmSection> IncludeNavigationProperties(IQueryable<UmSection> query)
        {
            return query.Include(o => o.Department).ThenInclude(a => a.Bureau).ThenInclude(b => b.Division);
        }
        protected override IQueryable<UmSection> ApplySearchFilter(IQueryable<UmSection> query, string searchQuery)
        {
            return query.Where(p => new[] { p.Name, p.Code, p.Description }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        public override IQueryable<UmSection> ApplyFilters(IQueryable<UmSection> query, List<Filter> filters)
        {
            if (filters != null && filters.Any())
            {
                // Apply each filter
                foreach (var filter in filters)
                {
                    if (filter.FilterOptions != null && filter.FilterOptions.Any())
                    {
                        // Apply filter using OR logic for FilterOptions
                        Expression<Func<UmSection, bool>> filterCondition = p => false; // Default false, will combine with OR

                        foreach (var option in filter.FilterOptions)
                        {
                            if (filter.FilterName.ToLower() == "department")
                            {
                                // Combine filter options with OR logic
                                var currentCondition = (Expression<Func<UmSection, bool>>)(p => p.DepartmentId == option.Value);
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

        protected override SectionDto MapToDto(UmSection entity)
        {
            var dto = new SectionDto
            {
                Id = entity.SectionId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                DepartmentId = entity.DepartmentId,
                Department = entity.Department == null ? null : new DepartmentDto
                {
                    Id = entity.Department.DepartmentId,
                    Code = entity.Department.Code,
                    Name = entity.Department.Name,
                    Description = entity.Department.Description,
                    IsActive = entity.Department.IsActive,
                    CreatedDate = entity.Department.CreatedDate,
                    CreatedBy = entity.Department.CreatedByUserId,
                    Bureau = entity.Department.Bureau == null ? null : new BureauDto
                    {
                        Id = entity.Department.Bureau.BureauId,
                        Code = entity.Department.Bureau.Code,
                        Name = entity.Department.Bureau.Name,
                        Description = entity.Department.Bureau.Description,
                        IsActive = entity.Department.Bureau.IsActive,
                        CreatedDate = entity.Department.Bureau.CreatedDate,
                        CreatedBy = entity.Department.Bureau.CreatedByUserId,
                        Division = entity.Department.Bureau.Division == null ? null : new DivisionDto
                        {
                            Id = entity.Department.Bureau.Division.DivisionId,
                            Code = entity.Department.Bureau.Division.Code,
                            Name = entity.Department.Bureau.Division.Name,
                            Description = entity.Department.Bureau.Division.Description,
                            IsActive = entity.Department.Bureau.Division.IsActive,
                            CreatedDate = entity.Department.Bureau.Division.CreatedDate,
                            CreatedBy = entity.Department.Bureau.Division.CreatedByUserId
                        }
                    }
                }
            };
            return dto;
        }

        protected override UmSection MapToEntity(SectionDto dto)
        {
            var entity = new UmSection
            {
                SectionId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy,
                DepartmentId = dto.DepartmentId
            };
            return entity;
        }
    }
}
