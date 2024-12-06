using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public partial class UmBureau
{
    public int BureauId { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? DivisionId { get; set; }

    public int? GroupId { get; set; }

    public virtual UmDivision? Division { get; set; }

    public virtual ICollection<UmDepartment> UmDepartments { get; set; } = new List<UmDepartment>();
}
