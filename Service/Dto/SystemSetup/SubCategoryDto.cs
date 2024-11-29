using System;
using System.Collections.Generic;

namespace Service.Dto.SystemSetup;

public partial class SubCategoryDto : BaseDto
{
    public int? Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }  

    public int? MajorCategoryId { get; set; }
    public string? MajorCategoryCode { get; set; }
    public string? MajorCategoryName { get; set; }
    public string? AccountCode { get; set; }
    public string? ItemTypeName { get; set; }

    public MajorCategoryDto? MajorCategory { get; set; }
}
