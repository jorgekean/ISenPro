using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class UmFilterCriterion
{
    public int FilterCriteriaId { get; set; }

    public int FilterBased { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? RoleId { get; set; }

    public int? UserAccountId { get; set; }

    public int? ParentModule { get; set; }

    public virtual UmRole? Role { get; set; }

    public virtual ICollection<UmFilterCriteriaList> UmFilterCriteriaLists { get; set; } = new List<UmFilterCriteriaList>();

    public virtual UmUserAccount? UserAccount { get; set; }
}
