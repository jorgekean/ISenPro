
CREATE VIEW dbo.v_PPMPIndex
AS
SELECT a.PPMPId,
       a.BudgetYear,
       CASE WHEN a.IsSubmitted = 1 THEN 'submitted' ELSE 'saved' END [Status],
       a.PPMPNo,
       d.Name OfficeName,
       p.FirstName + ', ' + p.LastName PreparedBy,
       a.Remarks,
       a.CreatedDate,
	   a.IsActive
FROM dbo.PPMPs a
    JOIN dbo.UM_Department d
        ON d.DepartmentId = a.RequestingOfficeId
    JOIN dbo.UM_UserAccount ua
        ON ua.UserAccountId = a.CreatedByUserId
	JOIN dbo.UM_Person p ON p.PersonId = ua.PersonId