using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class SsPsdbmcatalogueOffice
{
    public int PsdbmcatalogueOfficeId { get; set; }

    public int PsdbmcatalogueId { get; set; }

    public int DepartmentId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual UmDepartment Department { get; set; } = null!;

    public virtual SsPsdbmcatalogue Psdbmcatalogue { get; set; } = null!;
}
