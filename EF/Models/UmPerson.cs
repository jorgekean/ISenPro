using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class UmPerson
{
    public int PersonId { get; set; }

    public string? LastName { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? Address { get; set; }

    public bool IsHeadOfOffice { get; set; }

    public string? Email { get; set; }

    public string? ContactNo { get; set; }

    public string? Designation { get; set; }

    public int? EmployeeTitle { get; set; }

    public int? EmployeeStatus { get; set; }

    public bool IsActive { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }
}
