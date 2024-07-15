using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Service.Dto.UserManagement;
using Service.UserManagement.Interface;

namespace API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UmRole>>> GetRoles()
            {
            try
            {
                return Ok(await _roleService.GetAllAsync());
            }
            catch (Exception ex)
            {
                // logging here
                return BadRequest();
            }
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(string id)
        {
            var umRole = await _roleService.GetByIdAsync(id);

            if (umRole == null)
            {
                return NotFound();
            }

            return umRole;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(string id, RoleDto umRole)
        {
            if (id != umRole.Code)
            {
                return BadRequest();
            }

            //_roleService./*Entry*/(umRole).State = EntityState.Modified;
            //await _roleService.UpdateAsync(umRole);

            try
            {
                await _roleService.UpdateAsync(umRole);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await roleExists(id)))
                {
                    return NotFound();
                }
                else
                {
                    // logging here
                }
            }

            return NoContent();
        }

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UmRole>> PostUmRole(RoleDto umRole)
        {
            //_context.UmRoles.Add(umRole);
            try
            {
                await _roleService.AddAsync(umRole);
            }
            catch (DbUpdateException)
            {
                if (await roleExists(umRole.Code))
                {
                    return Conflict();
                }
                else
                {
                    // logging
                }
            }

            return CreatedAtAction("GetRole", new { id = umRole.Code }, umRole);
        }

        //// DELETE: api/Roles/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUmRole(string id)
        //{
        //    var umRole = await _context.UmRoles.FindAsync(id);
        //    if (umRole == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.UmRoles.Remove(umRole);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private async Task<bool> roleExists(string id)
        {
            return (await _roleService.GetAllAsync()).Any(e => e.Code == id);
        }
    }
}
