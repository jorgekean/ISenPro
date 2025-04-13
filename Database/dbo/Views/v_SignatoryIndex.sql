
CREATE VIEW dbo.v_SignatoryIndex
AS
 SELECT 
	a.SignatoryId as Id,
	b.[Name] as ModuleName,
	c.[Name] as SignatoryDesignation,
	d.[Name] as SignatoryOffice,
	e.LastName + ', ' + e.FirstName + ' ' + ISNULL(e.MiddleName, '') AS FullName,
	f.[Name] as Office,
	a.[Sequence],
	a.MinimumAmount,
	a.MaximumAmount,
	a.IsActive,
	a.CreatedDate
FROM dbo.SS_Signatories a
    LEFT JOIN dbo.UM_Module b
        ON a.Transactions = b.ModuleId
	LEFT JOIN dbo.SS_ReferenceTable c
		ON a.SignatoryDesignationId = c.ReferenceTableId
	LEFT JOIN dbo.SS_ReferenceTable d
		ON a.SignatoryOfficeId = d.ReferenceTableId
	LEFT JOIN dbo.UM_Person e
		ON a.PersonId = e.PersonId
	LEFT JOIN dbo.UM_Department f
		ON f.DepartmentId = e.DepartmentId