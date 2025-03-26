
CREATE VIEW dbo.v_PPMPIndex
AS
SELECT a.PPMPId,
       a.BudgetYear,
       a.Status,
       a.PPMPNo,
       d.Name OfficeName,
       p.FirstName + ', ' + p.LastName PreparedBy,
       a.Remarks,
       a.CreatedDate,
	   a.IsActive
FROM dbo.PPMPs a
    JOIN dbo.UM_Department d
        ON d.DepartmentId = a.RequestingOfficeId
    JOIN dbo.UM_Person p
        ON p.PersonId = a.CreatedByUserId;