﻿using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class UmPerson
{
    public int PersonId { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? Address { get; set; }

    public bool IsHeadOfOffice { get; set; }

    public string? Email { get; set; }

    public string? ContactNo { get; set; }

    public string? Thumbnail { get; set; }

    public string? Remarks { get; set; }

    public string? Designation { get; set; }

    public int? EmployeeTitle { get; set; }

    public int? EmployeeStatus { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? DepartmentId { get; set; }

    public int? SectionId { get; set; }

    public virtual UmDepartment? Department { get; set; }

    public virtual SsReferenceTable? EmployeeStatusNavigation { get; set; }

    public virtual SsReferenceTable? EmployeeTitleNavigation { get; set; }

    public virtual UmSection? Section { get; set; }

    public virtual ICollection<SsSignatory> SsSignatories { get; set; } = new List<SsSignatory>();

    public virtual UmUserAccount? UmUserAccount { get; set; }
}
