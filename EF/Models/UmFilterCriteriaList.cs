using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class UmFilterCriteriaList
{
    public int FilterCriteriaListId { get; set; }

    public int FilterCriteriaId { get; set; }

    public int CriteriaId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual UmFilterCriterion FilterCriteria { get; set; } = null!;
}
