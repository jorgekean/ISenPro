using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class VPersonIndex
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? EmployeeStatusLabel { get; set; }

    public string? BureauName { get; set; }

    public string? OfficeName { get; set; }

    public string? SectionName { get; set; }

    public bool IsHeadOfOffice { get; set; }

    public string? FullName { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedByUserId { get; set; }
}
