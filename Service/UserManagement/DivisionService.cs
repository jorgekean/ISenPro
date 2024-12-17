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
    public class DivisionService : BaseService<UmDivision, DivisionDto>, IDivisionService
    {
        public DivisionService(ISenProContext context) : base(context)
        {
        }

        //protected override IQueryable<UmDivision> IncludeNavigationProperties(IQueryable<UmDivision> query)
        //{
        //    return query.Include(o => o.UmBureaus);
        //}

        protected override IQueryable<UmDivision> ApplySearchFilter(IQueryable<UmDivision> query, string searchQuery)
        {
            return query.Where(p => new[] { p.Name, p.Code, p.Description }
                            .Any(value => value != null && value.Contains(searchQuery)));
        }

        protected override DivisionDto MapToDto(UmDivision entity)
        {
            var dto = new DivisionDto
            {
                Id = entity.DivisionId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedByUserId,
                //Bureaus = entity.UmBureaus.Select(b => new BureauDto
                //{
                //    Id = b.BureauId,
                //    Code = b.Code,
                //    Name = b.Name,
                //    Description = b.Description,
                //    IsActive = b.IsActive,
                //    CreatedDate = b.CreatedDate,
                //    CreatedBy = b.CreatedByUserId,
                //    DivisionId = b.DivisionId,
                //    GroupId = b.GroupId                    
                    
                //}).ToList()                 
            };
            return dto;
        }

        protected override UmDivision MapToEntity(DivisionDto dto)
        {
            var entity = new UmDivision
            {
                DivisionId = dto.Id.GetValueOrDefault(),
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                CreatedByUserId = dto.CreatedBy,                
            };
            return entity;
        }
    }
}
