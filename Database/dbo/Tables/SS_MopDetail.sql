CREATE TABLE [dbo].[SS_MopDetail] (
	[MopDetailId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsPercent] [bit] NULL,
	[Value] [float] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModeOfProcurementId] [int] NOT NULL,
    CONSTRAINT [PK_SS_MopDetail] PRIMARY KEY CLUSTERED ([MopDetailId] ASC),
	CONSTRAINT [FK_SS_ModeOfProcurement_SS_MopDetail] FOREIGN KEY ([ModeOfProcurementId]) REFERENCES [dbo].[SS_ModeOfProcurement] ([ModeOfProcurementId])
);

