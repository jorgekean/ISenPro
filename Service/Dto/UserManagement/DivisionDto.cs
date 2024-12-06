using Service.Dto;
using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public class DivisionDto : BaseDto
{
    public int? Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }


    public IEnumerable<BureauDto> Bureaus { get; set; } = new List<BureauDto>();
}
