using System;
using System.Collections.Generic;

namespace EF.Models.SystemSetup;

public partial class SsItemType
{
    public int ItemTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<SsAccountCode> SsAccountCodes { get; set; } = new List<SsAccountCode>();

    public virtual ICollection<SsPsdbmcatalogue> SsPsdbmcatalogues { get; set; } = new List<SsPsdbmcatalogue>();

    public virtual ICollection<SsSupplementaryCatalogue> SsSupplementaryCatalogues { get; set; } = new List<SsSupplementaryCatalogue>();
}
