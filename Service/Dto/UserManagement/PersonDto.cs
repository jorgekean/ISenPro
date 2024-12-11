using Service.Dto;
using System;
using System.Collections.Generic;

namespace EF.Models.UserManagement;

public class PersonDto : BaseDto
{
    public int? Id { get; set; }

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

    public string? EmployeeTitle { get; set; }

    public string? EmployeeStatus { get; set; }   

    public int? DepartmentId { get; set; }

    public int? SectionId { get; set; }

    public DepartmentDto? Department { get; set; }

    public SectionDto? Section { get; set; }


    public int? DivisionId { get; set; }
    public int? BureauId { get; set; }

    public string FullName
    {
        get
        {            
            return $"{LastName}, {FirstName} {MiddleName}";
        }
    }
}
