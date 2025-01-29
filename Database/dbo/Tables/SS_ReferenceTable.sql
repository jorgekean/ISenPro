CREATE TABLE [dbo].[SS_ReferenceTable](
	[ReferenceTableId] [int] IDENTITY(1,1) NOT NULL,
	[RefTableId] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
	[InflationValue] [decimal](19, 5) NULL,
	[IsActive] [bit] NULL,
	[CreatedByUserId] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
	CONSTRAINT [PK_SS_ReferenceTables] PRIMARY KEY CLUSTERED ([ReferenceTableId] ASC),
);