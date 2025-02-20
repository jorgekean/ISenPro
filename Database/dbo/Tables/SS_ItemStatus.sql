CREATE TABLE [dbo].[SS_ItemStatus] (
    [ItemStatusId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
    CONSTRAINT [PK_SS_ItemStatus] PRIMARY KEY CLUSTERED ([ItemStatusId] ASC)
);