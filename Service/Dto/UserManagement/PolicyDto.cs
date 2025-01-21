using Service.Dto;
using Service.Dto.UserManagement;
using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public class PolicyDto : BaseDto
{
    public int? Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
    public List<PolicyRoleDto> Roles { get; set; } = new List<PolicyRoleDto>();
    public List<PolicyModuleControlDto> ModuleControls { get; set; } = new List<PolicyModuleControlDto>();


}
