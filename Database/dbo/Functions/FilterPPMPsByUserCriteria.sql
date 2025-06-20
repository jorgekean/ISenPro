
CREATE FUNCTION dbo.FilterPPMPsByUserCriteria
(
    @userId INT,
    @parentModule INT = 1
)
RETURNS TABLE
AS
RETURN
(
    -- Get user info (admin status)
    WITH UserInfo AS (
        SELECT 
            ua.UserAccountId,
            ua.IsAdmin
        FROM dbo.UM_UserAccount ua
        WHERE ua.UserAccountId = @userId
    )
    
    SELECT 
        p.PPMPId,
        p.BudgetYear,
        p.Status AS [Status],
        p.PPMPNo,
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
    CROSS JOIN
        UserInfo ui
    WHERE
        dbo.ApplyTransactionFilters(
            @userId,
            p.RequestingOfficeId,
            p.CreatedByUserId,
            p.Status,
            ui.IsAdmin,
            @parentModule
        ) = 1
)