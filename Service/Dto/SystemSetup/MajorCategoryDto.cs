using EF.Models.SystemSetup;
using System;
using System.Collections.Generic;

namespace Service.Dto.SystemSetup
{
    public partial class MajorCategoryDto : BaseDto
    {
        public int? Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? AccountCodeId { get; set; }

        public string? AccountCodeCode { get; set; }
        public AccountCodeDto? AccountCode { get; set; }

        //public virtual ICollection<SsSubCategory> SsSubCategories { get; set; } = new List<SsSubCategory>();
    }
}