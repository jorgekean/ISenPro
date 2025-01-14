using Service.Dto;
using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public class PolicyDto : BaseDto
{
    public int? Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

   
}
