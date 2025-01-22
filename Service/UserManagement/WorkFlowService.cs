using EF;
using EF.Models;
using EF.Models.SystemSetup;
using EF.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Service.Dto.SystemSetup;
using Service.Dto.UserManagement;
using Service.Service;
using Service.SystemSetup.Interface;
using Service.UserManagement.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Service.UserManagement
{
    public class WorkFlowService : BaseService<UmWorkFlow, WorkFlowDto>, IWorkFlowService
    {
        public WorkFlowService(ISenProContext context) : base(context)
        {

        }

        //protected override IQueryable<UmWorkFlow> IncludeNavigationProperties(IQueryable<UmWorkFlow> query)
        //{
            //return query.Include(o => o.ItemType);
        //}

        public override IQueryable<UmWorkFlow> ApplyFilters(IQueryable<UmWorkFlow> query, List<Filter> filters)
        {
            if (filters != null && filters.Any())
            {
                // Apply each filter
                foreach (var filter in filters)
                {
                    if (filter.FilterOptions != null && filter.FilterOptions.Any())
                    {
                        // Apply filter using OR logic for FilterOptions
                        Expression<Func<UmWorkFlow, bool>> filterCondition = p => false; // Default false, will combine with OR

                        foreach (var option in filter.FilterOptions)
                        {
                            if (filter.FilterName.ToLower() == "itemtype")
                            {
                                // Combine filter options with OR logic
                                //var currentCondition = (Expression<Func<UmWorkFlow, bool>>)(p => p.ItemTypeId == option.Value);
                                //filterCondition = CombineWithOr(filterCondition, currentCondition);
                            }
                        }

                        // Apply the OR condition to the query
                        query = query.Where(filterCondition);
                    }
                }
            }

            return query;
        }

        protected override IQueryable<UmWorkFlow> ApplySearchFilter(IQueryable<UmWorkFlow> query, string searchQuery)
        {
            return query.Where(p => new[] { p.Description, p.Code }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        public Task<List<WorkStepDto>> GetWorkSteps(int workFlowId)
        {
            var workSteps = _context.UmWorkSteps.Where(x => x.WorkflowId == workFlowId).Select(x => new WorkStepDto
            {
                Id = x.WorkstepId,
                WorkflowId = x.WorkflowId,
                Sequence = x.Sequence,
                Name = x.Name,
                Description = x.Description,
                RequiredApprover = x.RequiredApprover,
                CanModify = x.CanModify,
                IsLastStep = x.IsLastStep,
                IsActive = x.IsActive
            }).ToListAsync();

            return workSteps;
        }

        protected override WorkFlowDto MapToDto(UmWorkFlow entity)
        {
            var dto = new WorkFlowDto
            {
                Id = entity.WorkflowId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                WorkSteps = entity.UmWorkSteps.Select(x => new WorkStepDto
                {
                    Id = x.WorkstepId,
                    WorkflowId = x.WorkflowId,
                    Sequence = x.Sequence,
                    Name = x.Name,
                    Description = x.Description,
                    RequiredApprover = x.RequiredApprover,
                    CanModify = x.CanModify,
                    IsLastStep = x.IsLastStep,
                    IsActive = x.IsActive
                })
            };

            if (entity.ModuleId.HasValue)
            {
                var module = _context.UmModules.FirstOrDefault(x => x.ModuleId == entity.ModuleId.Value);

                dto.ModuleName = module != null ? module.Name : string.Empty;
                dto.ModuleId = entity.ModuleId.Value;
            }

            return dto;
        }

        protected override UmWorkFlow MapToEntity(WorkFlowDto dto)
        {
            var entity = new UmWorkFlow
            {
                WorkflowId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                ModuleId = dto.ModuleId,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedByUserId = 1
            };

            return entity;
        }
    }
}
