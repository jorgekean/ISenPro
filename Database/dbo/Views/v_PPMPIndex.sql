
CREATE VIEW dbo.v_PPMPIndex
AS
SELECT 
    p.PPMPId,
    p.BudgetYear,	  
    p.Status AS [Status],
    p.PPMPNo,
	p.RequestingOfficeId,
	p.CreatedByUserId,
    d.Name AS OfficeName,
    CONCAT(per.FirstName, ', ', per.LastName) AS PreparedBy,
    p.Remarks,
    p.CreatedDate,
    p.IsActive
FROM 
    dbo.PPMPs p
INNER JOIN 
    dbo.UM_Department d ON d.DepartmentId = p.RequestingOfficeId
INNER JOIN 
    dbo.UM_UserAccount ua ON ua.UserAccountId = p.CreatedByUserId
INNER JOIN 
    dbo.UM_Person per ON per.PersonId = ua.PersonId
--OUTER APPLY (
--    SELECT TOP (1) 
--        ts.Status,
--        ts.Action,
--        ts.TransactionStatusId
--    FROM 
--        dbo.TransactionStatuses ts 
--    WHERE 
--        ts.PageId = 25 
--        AND ts.TransactionId = p.PPMPId 
--        AND (ts.IsActive = 1 OR ts.Action = 'disapproved')
--    ORDER BY 
--        ts.TransactionStatusId DESC
--) ts;