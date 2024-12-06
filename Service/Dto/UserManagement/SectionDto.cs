using Service.Dto;
using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public class SectionDto : BaseDto
{
    public int? Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }   

    public int? DepartmentId { get; set; }

    public DepartmentDto? Department { get; set; }

    public IEnumerable<PersonDto> People { get; set; } = new List<PersonDto>();
}
