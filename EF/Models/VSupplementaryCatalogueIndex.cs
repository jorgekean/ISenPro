using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class VSupplementaryCatalogueIndex
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public decimal? UnitPrice { get; set; }

    public string? UnitOfMeasurementCode { get; set; }

    public string? ItemTypeName { get; set; }

    public string? MajorCategoryName { get; set; }

    public string? SubCategoryName { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }
}
