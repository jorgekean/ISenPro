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

        public override async Task<object> AddAsync(WorkStepDto dto)
        {
            var entity = MapToEntity(dto);

            //// Track child entities generically
            //TrackChildEntities(entity);

            // Add the parent entity
            var result = await _dbSet.AddAsync(entity);

            // Save changes for parent
            await _context.SaveChangesAsync();

            #region Approvers

            // loop through dto.WorkStepApprovers
            dto.WorkStepApprovers.ForEach(item =>
            {
                var workStepApprover = new UmWorkStepApprover
                {
                    WorkstepId = entity.WorkstepId,
                    UserAccountId = item.UserAccountId,
                    CreatedByUserId = entity.CreatedByUserId,
                    CreatedDate = entity.CreatedDate,
                    IsActive = item.IsActive
                };
                _context.UmWorkStepApprovers.Add(workStepApprover);
            });

            await _context.SaveChangesAsync();

            #endregion

            return result.Entity;
        }

        public override async Task UpdateAsync(WorkStepDto dto)
        {
            var entity = MapToEntity(dto);

            // update parent
            _dbSet.Update(entity);

            #region Approvers

            // update child roles: delete then insert
            _context.UmWorkStepApprovers.RemoveRange(_context.UmWorkStepApprovers.Where(x => x.WorkstepId == entity.WorkstepId));

            // loop through dto.WorkStepApprovers
            dto.WorkStepApprovers.ForEach(item =>
            {
                var workStepApprover = new UmWorkStepApprover
                {
                    WorkstepId = entity.WorkstepId,
                    UserAccountId = item.UserAccountId,
                    CreatedByUserId = entity.CreatedByUserId,
                    CreatedDate = entity.CreatedDate,
                    IsActive = item.IsActive
                };
                _context.UmWorkStepApprovers.Add(workStepApprover);
            });

            await _context.SaveChangesAsync();

            #endregion
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            await _context.Entry(entity).Collection(x => x.UmWorkStepApprovers).LoadAsync();

            if (entity != null)
            {
                foreach (var item in entity.UmWorkStepApprovers)
                {
                    _context.Entry(item).Property("IsActive").CurrentValue = false;
                }

                _context.Entry(entity).Property("IsActive").CurrentValue = false;
                await _context.SaveChangesAsync();
            }
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
