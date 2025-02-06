CREATE TABLE [dbo].[SS_SubStatus] (
	[SubStatusId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ItemStatusId] [int] NOT NULL,
    CONSTRAINT [PK_SS_SubStatus] PRIMARY KEY CLUSTERED ([SubStatusId] ASC),
	CONSTRAINT [FK_SS_ItemStatus_SS_SubStatus] FOREIGN KEY ([ItemStatusId]) REFERENCES [dbo].[SS_ItemStatus] ([ItemStatusId])
);