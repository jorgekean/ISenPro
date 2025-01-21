using Service.Dto;
using Service.Dto.UserManagement;
using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public class PolicyModuleControlDto : BaseDto
{
    public int Id { get; set; }

    public int? PolicyId { get; set; }
    public bool IsChecked { get; set; }    

    public int ModuleControlId { get; set; }

    //public ModuleControlDto ModuleControl { get; set; } = null!;
}
