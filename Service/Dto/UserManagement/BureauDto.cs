using Service.Dto;
using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public class BureauDto : BaseDto
{
    public int? Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? DivisionId { get; set; }

    public int? GroupId { get; set; }

    public DivisionDto? Division { get; set; }

    public virtual IEnumerable<DepartmentDto> UmDepartments { get; set; } = new List<DepartmentDto>();
}
