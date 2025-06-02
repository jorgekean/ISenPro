

CREATE VIEW [dbo].[v_TransactionHistory]
AS
SELECT A.TransactionStatusId,
       A.TransactionId,
	   A.PageId,
       ws.Description,
       ua.UserID,
       p.LastName + ', ' + p.FirstName AS DisplayName,
       A.Remarks,
       A.Action,
       A.CreatedDate
FROM dbo.TransactionStatuses A
    JOIN dbo.UM_WorkStep ws
        ON ws.WorkstepId = A.WorkstepId
    JOIN dbo.UM_UserAccount ua
        ON ua.UserAccountId = A.ProcessByUserId
    JOIN dbo.UM_Person p
        ON p.PersonId = ua.PersonId