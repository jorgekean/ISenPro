using System;
using System.Collections.Generic;

namespace EF.Models.SystemSetup;

public partial class SsMajorCategory
{
    public int MajorCategoryId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? AccountCodeId { get; set; }

    public virtual SsAccountCode? AccountCode { get; set; }

    public virtual ICollection<SsPsdbmcatalogue> SsPsdbmcatalogues { get; set; } = new List<SsPsdbmcatalogue>();

    public virtual ICollection<SsSubCategory> SsSubCategories { get; set; } = new List<SsSubCategory>();

    public virtual ICollection<SsSupplementaryCatalogue> SsSupplementaryCatalogues { get; set; } = new List<SsSupplementaryCatalogue>();
}
