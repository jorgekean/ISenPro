CREATE FUNCTION dbo.ApplyTransactionFilters
(
    @userId INT,
    @officeId INT,
    @createdByUserId INT,
    @status VARCHAR(50),
    @isAdmin BIT,
    @parentModule INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @showRecord BIT = 0;
    
    -- Get user's department
    DECLARE @userDeptId INT = (
        SELECT p.DepartmentId 
        FROM dbo.UM_UserAccount a
        JOIN dbo.UM_Person p ON p.PersonId = a.PersonId
        WHERE a.UserAccountId = @userId
    );
    
    -- If admin, always show
    IF @isAdmin = 1
        RETURN 1;
    
    -- Get filter criteria
    IF NOT EXISTS (SELECT 1 FROM dbo.GetUserFilterCriteria(@userId, @parentModule))
        RETURN 1; -- No filters = show all
    
    -- Check each filter criteria
    DECLARE @criteria11 BIT = CASE WHEN EXISTS (
        SELECT 1 FROM dbo.GetUserFilterCriteria(@userId, @parentModule) WHERE CriteriaId = 11
    ) THEN 1 ELSE 0 END;
    
    DECLARE @criteria12 BIT = CASE WHEN EXISTS (
        SELECT 1 FROM dbo.GetUserFilterCriteria(@userId, @parentModule) WHERE CriteriaId = 12
    ) THEN 1 ELSE 0 END;
    
    DECLARE @criteria14 BIT = CASE WHEN EXISTS (
        SELECT 1 FROM dbo.GetUserFilterCriteria(@userId, @parentModule) WHERE CriteriaId = 14
    ) THEN 1 ELSE 0 END;
    
    -- Apply filter logic
    IF @criteria11 = 1 AND @officeId = @userDeptId
        SET @showRecord = 1;
    ELSE IF @criteria12 = 1 AND @createdByUserId = @userId
        SET @showRecord = 1;
    ELSE IF @criteria14 = 1 AND @status = 'Approved'
        SET @showRecord = 1;
    ELSE IF @criteria11 = 0 AND @criteria12 = 0 AND @criteria14 = 0
        SET @showRecord = 1; -- No applicable filters
    
    RETURN @showRecord;
END