
CREATE FUNCTION dbo.fn_GetUserTransactionPermissions
(
    @TransactionId INT,
    @UserAccountId INT,
    @ModuleId INT = 25
)
RETURNS @Results TABLE 
(
    CanApprove BIT,
    CanModify BIT
)
AS
BEGIN
    DECLARE @CurrentSequence INT;
    DECLARE @CanModify BIT = 0;
    DECLARE @CanApprove BIT = 0;
    
    -- Determine current sequence
    IF NOT EXISTS (SELECT 1 FROM TransactionStatuses WHERE PageId=@ModuleId AND TransactionId = @TransactionId)
    BEGIN
        -- New transaction - use first sequence
        SET @CurrentSequence = 1;
    END
    ELSE
    BEGIN
        -- Get highest active sequence
        SELECT TOP 1 @CurrentSequence = CASE WHEN ts.IsDone = 1 THEN ws.Sequence + 1 ELSE ws.Sequence END
        FROM TransactionStatuses ts
        JOIN UM_WorkStep ws ON ts.WorkstepId = ws.WorkstepId
        WHERE ts.TransactionId = @TransactionId AND ts.PageId=@ModuleId
        AND ts.IsCurrent = 1
        ORDER BY ws.Sequence DESC;
    END
    
    -- Check if user can approve current sequence
    IF EXISTS (
        SELECT 1 
        FROM UM_WorkStep ws
        JOIN UM_Workflow wf ON ws.WorkflowId = wf.WorkflowId
        JOIN UM_WorkStepApprover wsa ON ws.WorkstepId = wsa.WorkstepId
        WHERE wf.ModuleId = @ModuleId
        AND ws.Sequence = @CurrentSequence
        AND wsa.UserAccountId = @UserAccountId
        AND wsa.IsActive = 1
    )
    BEGIN
        SET @CanApprove = 1;
    END
    
    -- Get CanModify for current sequence
    SELECT @CanModify = ISNULL((
        SELECT TOP 1 ws.CanModify
        FROM UM_WorkStep ws
        JOIN UM_Workflow wf ON ws.WorkflowId = wf.WorkflowId
        WHERE wf.ModuleId = @ModuleId
        AND ws.Sequence = @CurrentSequence
    ), 0);
    
    -- Return results
    INSERT INTO @Results (CanApprove, CanModify)
    VALUES (@CanApprove, @CanModify);
    
    RETURN;
END