
CREATE VIEW dbo.v_PersonIndex
AS
SELECT 
    a.PersonId as Id,
    a.LastName, 
    a.FirstName, 
    a.MiddleName, 
    r.[Name] as EmployeeStatusLabel, 
    b.[Name] as BureauName, 
    d.[Name] as OfficeName, 
    s.[Name] as SectionName,
    a.IsHeadOfOffice,
    a.LastName + ', ' + a.FirstName + ' ' + ISNULL(a.MiddleName, '') as FullName,
    a.IsActive,
    a.CreatedDate,
    a.CreatedByUserId
FROM dbo.UM_Person a
    JOIN dbo.UM_Department d
        ON d.DepartmentId = a.DepartmentId
    JOIN dbo.UM_Section s
        ON s.SectionId = a.SectionId
    JOIN dbo.UM_Bureau b
        ON b.BureauId = d.BureauId
	JOIN dbo.SS_ReferenceTable r
		ON r.ReferenceTableId = a.EmployeeStatus;