CREATE VIEW dbo.v_UserAccountIndex
AS
SELECT 
    u.UserAccountId as Id,
	u.UserID,
    a.LastName, 
    a.FirstName, 
    a.MiddleName, 
    r.[Name] as EmployeeStatusLabel, 
    b.[Name] as BureauName, 
    d.[Name] as OfficeName, 
    s.[Name] as SectionName,
    a.LastName + ', ' + a.FirstName + ' ' + ISNULL(a.MiddleName, '') as FullName,
	ro.Code as RoleName,
    a.IsActive,
    a.CreatedDate,
    a.CreatedByUserId
FROM dbo.UM_UserAccount u
    JOIN dbo.UM_Person a
        ON u.PersonId = a.PersonId
    JOIN dbo.UM_Department d
        ON d.DepartmentId = a.DepartmentId
    JOIN dbo.UM_Section s
        ON s.SectionId = a.SectionId
    JOIN dbo.UM_Bureau b
        ON b.BureauId = d.BureauId
	JOIN dbo.SS_ReferenceTable r
		ON r.ReferenceTableId = a.EmployeeStatus
	JOIN dbo.UM_Role ro
		ON u.RoleId = ro.RoleId;