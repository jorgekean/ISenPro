CREATE FUNCTION dbo.GetUserFilterCriteria
(
    @userId INT,
    @parentModule INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT TOP 1 
        fcl.CriteriaId
    FROM dbo.UM_FilterCriteria fc
    JOIN dbo.UM_FilterCriteriaList fcl ON fc.FilterCriteriaId = fcl.FilterCriteriaId
    WHERE fc.IsActive = 1  AND fcl.IsActive=1
      AND fc.ParentModule = @parentModule
      AND (
          (fc.UserAccountId = @userId) OR
          (fc.UserAccountId IS NULL AND fc.RoleId IN (
              SELECT RoleId FROM dbo.UM_UserAccount WHERE UserAccountId = @userId
          ))
      )
    ORDER BY CASE WHEN fc.UserAccountId IS NOT NULL THEN 0 ELSE 1 END
)