CREATE TABLE [dbo].[SS_ModeOfProcurement] (
    [ModeOfProcurementId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[WithCondition] [bit] NULL,
	[MinimumAmount] [float] NULL,
	[MaximumAmount] [float] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
    CONSTRAINT [PK_SS_ModeOfProcurement] PRIMARY KEY CLUSTERED ([ModeOfProcurementId] ASC)
);

