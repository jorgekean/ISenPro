using System;
using System.Collections.Generic;

namespace EF.Models.SystemSetup;

public partial class SsSubCategory
{
    public int SubCategoryId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? MajorCategoryId { get; set; }

    public virtual SsMajorCategory? MajorCategory { get; set; }

    public virtual ICollection<SsPsdbmcatalogue> SsPsdbmcatalogues { get; set; } = new List<SsPsdbmcatalogue>();

    public virtual ICollection<SsSupplementaryCatalogue> SsSupplementaryCatalogues { get; set; } = new List<SsSupplementaryCatalogue>();
}
