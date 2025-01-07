using EF.Models.UserManagement;
using System;
using System.Collections.Generic;

namespace EF.Models.SystemSetup;

public partial class SsSignatory
{
    public int SignatoryId { get; set; }

    public int? Transactions { get; set; }

    public int? Sequence { get; set; }

    public int? SignatoryDesignationId { get; set; }

    public int? SignatoryOfficeId { get; set; }

    public int? ReportSectionId { get; set; }

    public bool? WithCondition { get; set; }

    public double? MinimumAmount { get; set; }

    public double? MaximumAmount { get; set; }

    public bool? IsActive { get; set; }

    public long? CreatedByUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? PersonId { get; set; }

    public virtual UmPerson? Person { get; set; }

    public virtual SsReferenceTable? ReportSection { get; set; }

    public virtual SsReferenceTable? SignatoryDesignation { get; set; }
}
