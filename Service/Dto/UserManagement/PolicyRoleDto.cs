using Service.Dto;
using Service.Dto.UserManagement;
using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public class PolicyRoleDto : BaseDto
{
    public int? Id { get; set; }

    public int PolicyId { get; set; }   
    public int RoleId { get; set; }

    public RoleDto Role { get; set; } = null!;
}
