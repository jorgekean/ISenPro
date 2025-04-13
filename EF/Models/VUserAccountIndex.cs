using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class VUserAccountIndex
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? EmployeeStatusLabel { get; set; }

    public string? BureauName { get; set; }

    public string? OfficeName { get; set; }

    public string? SectionName { get; set; }

    public string? FullName { get; set; }

    public string RoleName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedByUserId { get; set; }
}
