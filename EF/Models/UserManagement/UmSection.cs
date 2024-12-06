using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public partial class UmSection
{
    public int SectionId { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? DepartmentId { get; set; }

    public virtual UmDepartment? Department { get; set; }

    public virtual ICollection<UmPerson> UmPeople { get; set; } = new List<UmPerson>();
}
