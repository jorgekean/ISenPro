using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class SsSupplementaryCatalogue
{
    public int SupplementaryCatalogueId { get; set; }

    public DateTime? CatalogueYear { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public decimal? UnitPrice { get; set; }

    public bool? IsCurrent { get; set; }

    public string? Remarks { get; set; }

    public string? Thumbnail { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ItemTypeId { get; set; }

    public int? AccountCodeId { get; set; }

    public int? MajorCategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public int? UnitOfMeasurementId { get; set; }

    public bool? IsOriginal { get; set; }

    public virtual SsAccountCode? AccountCode { get; set; }

    public virtual SsItemType? ItemType { get; set; }

    public virtual SsMajorCategory? MajorCategory { get; set; }

    public virtual ICollection<Ppmpsupplementary> Ppmpsupplementaries { get; set; } = new List<Ppmpsupplementary>();

    public virtual SsSubCategory? SubCategory { get; set; }

    public virtual SsUnitOfMeasurement? UnitOfMeasurement { get; set; }
}
