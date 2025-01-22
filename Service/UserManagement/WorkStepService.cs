using EF;
using EF.Models;
using EF.Models.UserManagement;
using Service.Dto.UserManagement;
using Service.Service;
using Service.UserManagement.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UserManagement
{
    public class WorkStepService : BaseService<UmWorkStep, WorkStepDto>, IWorkStepService
    {
        public WorkStepService(ISenProContext context) : base(context)
        {
        }

        protected override IQueryable<UmWorkStep> ApplySearchFilter(IQueryable<UmWorkStep> query, string searchQuery)
        {
            return query.Where(p => new[] { p.Name, p.Description }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        protected override WorkStepDto MapToDto(UmWorkStep entity)
        {
            var dto = new WorkStepDto
            {
                Id = entity.WorkstepId,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate
            };
            return dto;
        }

        protected override UmWorkStep MapToEntity(WorkStepDto dto)
        {
            var entity = new UmWorkStep
            {
                WorkstepId = dto.Id.GetValueOrDefault(),
                Name = dto.Name,
                Description = dto.Description,
                CanModify = dto.CanModify,
                IsLastStep = dto.IsLastStep,
                RequiredApprover = dto.RequiredApprover,
                Sequence = dto.Sequence,
                WorkflowId = dto.WorkflowId.GetValueOrDefault(),
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedByUserId = 1
            };

            return entity;
        }
    }
}
