using Service.Dto;
using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public class DepartmentDto : BaseDto
{
    public int? Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? ResponsibilityCenter { get; set; }  

    public int? BureauId { get; set; }

    public BureauDto? Bureau { get; set; }

    public IEnumerable<PersonDto> People { get; set; } = new List<PersonDto>();

    public IEnumerable<SectionDto> Sections { get; set; } = new List<SectionDto>();
}
