using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public partial class UmDepartment
{
    public int DepartmentId { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? ResponsibilityCenter { get; set; }

    public bool IsActive { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? BureauId { get; set; }

    public virtual UmBureau? Bureau { get; set; }

    public virtual ICollection<UmPerson> UmPeople { get; set; } = new List<UmPerson>();

    public virtual ICollection<UmSection> UmSections { get; set; } = new List<UmSection>();
}
