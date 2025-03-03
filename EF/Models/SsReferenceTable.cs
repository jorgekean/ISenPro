using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class SsReferenceTable
{
    public int ReferenceTableId { get; set; }

    public int? RefTableId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Code { get; set; }

    public decimal? InflationValue { get; set; }

    public bool? IsActive { get; set; }

    public long? CreatedByUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<SsSignatory> SsSignatoryReportSections { get; set; } = new List<SsSignatory>();

    public virtual ICollection<SsSignatory> SsSignatorySignatoryDesignations { get; set; } = new List<SsSignatory>();

    public virtual ICollection<SsSignatory> SsSignatorySignatoryOffices { get; set; } = new List<SsSignatory>();

    public virtual ICollection<UmPerson> UmPersonEmployeeStatusNavigations { get; set; } = new List<UmPerson>();

    public virtual ICollection<UmPerson> UmPersonEmployeeTitleNavigations { get; set; } = new List<UmPerson>();
}
