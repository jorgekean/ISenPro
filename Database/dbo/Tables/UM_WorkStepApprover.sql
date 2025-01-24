CREATE TABLE [dbo].[UM_WorkStepApprover] (
	[WorkstepApproverId] [int] IDENTITY(1,1) NOT NULL,
	[WorkstepId] [int] NOT NULL,
	[UserAccountId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,

    CONSTRAINT [PK_UM_WorkStepApprover] PRIMARY KEY CLUSTERED ([WorkstepApproverId] ASC),
	CONSTRAINT [FK_UM_WorkStepApprovers_UM_WorkSteps] FOREIGN KEY ([WorkstepId]) REFERENCES [dbo].[UM_WorkStep] ([WorkstepId]),
	CONSTRAINT [FK_UM_WorkStepApprovers_UM_UserAccounts] FOREIGN KEY ([UserAccountId]) REFERENCES [dbo].[UM_UserAccount] ([UserAccountId])
);