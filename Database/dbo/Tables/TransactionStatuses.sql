CREATE TABLE [dbo].[TransactionStatuses] (
    [TransactionStatusId] INT            IDENTITY (1, 1) NOT NULL,
    [PageId]              INT            NOT NULL,
    [ProcessByUserId]     INT            NOT NULL,
    [IsCurrent]           BIT            NOT NULL,
    [Count]               INT            NOT NULL,
    [Status]              NVARCHAR (200) NULL,
    [Remarks]             NVARCHAR (MAX) NULL,
    [CreatedDate]         DATETIME       NOT NULL,
    [TransactionId]       INT            NOT NULL,
    [IsDone]              BIT            NOT NULL,
    [WorkstepId]          INT            NULL,
    [IsActive]            BIT            NOT NULL,
    [Action]              NVARCHAR (20)  NULL,
    CONSTRAINT [PK__Transact__57B5E1832ACC04F9] PRIMARY KEY CLUSTERED ([TransactionStatusId] ASC)
);






GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20201119-202802]
    ON [dbo].[TransactionStatuses]([TransactionId] ASC, [WorkstepId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Will be disabled(0) if transaction is disapproved. Default value is true(1)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransactionStatuses', @level2type = N'COLUMN', @level2name = N'IsActive';

