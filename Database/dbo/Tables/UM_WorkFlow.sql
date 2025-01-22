CREATE TABLE [dbo].[UM_WorkFlow] (
    [WorkflowId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
    [ModuleId] [int] NULL

    CONSTRAINT [PK_UM_WorkFlows] PRIMARY KEY CLUSTERED ([WorkflowId] ASC),
	CONSTRAINT [FK_UM_WorkFlows_UM_Modules] FOREIGN KEY ([ModuleId]) REFERENCES [dbo].[UM_Module] ([ModuleId])
);