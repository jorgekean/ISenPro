CREATE TABLE [dbo].[UM_WorkStep] (
    [WorkstepId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Sequence] [int] NULL,
	[IsLastStep] [bit] NULL,
	[RequiredApprover] [int] NULL,
	[CanModify] [bit] NULL,
	[WorkflowId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL

    CONSTRAINT [PK_UM_WorkStep] PRIMARY KEY CLUSTERED ([WorkstepId] ASC),
	CONSTRAINT [FK_UM_WorkSteps_UM_WorkFlows] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[UM_WorkFlow] ([WorkflowId])
);