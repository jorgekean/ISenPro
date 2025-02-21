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
    CONSTRAINT [PK__Transact__57B5E1832ACC04F9] PRIMARY KEY CLUSTERED ([TransactionStatusId] ASC),
    CONSTRAINT [FK955A368C57BC1BE3] FOREIGN KEY ([WorkstepId]) REFERENCES [dbo].[UM_WorkStep] ([WorkstepId])
);


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20201119-202802]
    ON [dbo].[TransactionStatuses]([TransactionId] ASC, [WorkstepId] ASC);

