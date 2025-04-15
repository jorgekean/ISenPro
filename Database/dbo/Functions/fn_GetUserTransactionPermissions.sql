
CREATE FUNCTION dbo.fn_GetUserTransactionPermissions
(
    @TransactionId INT,        -- ID of the transaction being checked
    @UserAccountId INT,        -- ID of the user whose permissions are being verified
    @ModuleId INT = 25         -- Module ID (defaults to 25 if not specified)
)
RETURNS @Results TABLE 
(
    WorkStepId INT,            -- ID of the current workstep
    CanApprove BIT,            -- Flag indicating if user can approve at this step
    CanModify BIT,             -- Flag indicating if user can modify at this step
    WorkStepName NVARCHAR(100),-- Descriptive name of the current workstep
    RequiredApprover INT       -- Indicates if this step requires approval
)
AS
BEGIN
    -- Variable declarations
    DECLARE @CurrentSequence INT;      -- Tracks the current workflow sequence number
    DECLARE @CanModify BIT = 0;        -- Permission flag (default: false)
    DECLARE @CanApprove BIT = 0;       -- Permission flag (default: false)
    DECLARE @WorkStepId INT = 0;       -- Stores the current workstep ID
    DECLARE @WorkStepName NVARCHAR(100) = ''; -- Stores the workstep name
    DECLARE @RequiredApprover INT = 0; -- Stores if approval is required for this step
    
    -- ========================================================================
    -- Determine the current workflow position
    -- ========================================================================
    -- Check if this is a brand new transaction (no status records yet)
    IF NOT EXISTS (SELECT 1 FROM TransactionStatuses 
                  WHERE IsActive = 1 AND PageId = @ModuleId AND TransactionId = @TransactionId)
    BEGIN
        -- For new transactions, get the FIRST step in the workflow
        SELECT TOP 1 
            @CurrentSequence = ws.Sequence,
            @WorkStepId = ws.WorkstepId,
            @WorkStepName = ws.Name,
            @RequiredApprover = ws.RequiredApprover
        FROM UM_WorkStep ws
        JOIN UM_Workflow wf ON ws.WorkflowId = wf.WorkflowId
        WHERE wf.ModuleId = @ModuleId
        ORDER BY ws.Sequence;
    END
    ELSE
    BEGIN
        -- For existing transactions, find the CURRENT or NEXT active step
        SELECT TOP 1 
            @CurrentSequence = CASE 
                WHEN ts.IsDone = 1 THEN ws.Sequence + 1  -- Move to next if current is done
                ELSE ws.Sequence END,                    -- Stay on current if not done
            @WorkStepId = CASE 
                WHEN ts.IsDone = 1 THEN 
                    (SELECT TOP 1 ws2.WorkstepId 
                     FROM UM_WorkStep ws2 
                     JOIN UM_Workflow wf2 ON ws2.WorkflowId = wf2.WorkflowId
                     WHERE wf2.ModuleId = @ModuleId AND ws2.Sequence = ws.Sequence + 1)
                ELSE ws.WorkstepId END,
            @WorkStepName = CASE
                WHEN ts.IsDone = 1 THEN
                    (SELECT TOP 1 ws2.Name
                     FROM UM_WorkStep ws2 
                     JOIN UM_Workflow wf2 ON ws2.WorkflowId = wf2.WorkflowId
                     WHERE wf2.ModuleId = @ModuleId AND ws2.Sequence = ws.Sequence + 1)
                ELSE ws.Name END,
            @RequiredApprover = CASE
                WHEN ts.IsDone = 1 THEN
                    (SELECT TOP 1 ws2.RequiredApprover
                     FROM UM_WorkStep ws2 
                     JOIN UM_Workflow wf2 ON ws2.WorkflowId = wf2.WorkflowId
                     WHERE wf2.ModuleId = @ModuleId AND ws2.Sequence = ws.Sequence + 1)
                ELSE ws.RequiredApprover END
        FROM TransactionStatuses ts
        JOIN UM_WorkStep ws ON ts.WorkstepId = ws.WorkstepId
        WHERE ts.IsActive = 1 AND ts.TransactionId = @TransactionId 
          AND ts.PageId = @ModuleId
        ORDER BY ws.Sequence DESC;
    END
    
    -- ========================================================================
    -- Check approval permissions for this user at current step
    -- ========================================================================
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
        SET @CanApprove = 1;  -- User is an approver for this step
    END
    
    -- ========================================================================
    -- Check modification permissions for current step
    -- ========================================================================
    SELECT @CanModify = ISNULL((
        SELECT TOP 1 ws.CanModify
        FROM UM_WorkStep ws
        JOIN UM_Workflow wf ON ws.WorkflowId = wf.WorkflowId
        WHERE wf.ModuleId = @ModuleId
          AND ws.Sequence = @CurrentSequence
    ), 0);  -- Default to false if no record found
    
    -- ========================================================================
    -- Return all results including the workstep name and required approver flag
    -- ========================================================================
    INSERT INTO @Results (WorkStepId, CanApprove, CanModify, WorkStepName, RequiredApprover)
    VALUES (@WorkStepId, @CanApprove, @CanModify, @WorkStepName, @RequiredApprover);
    
    RETURN;
END