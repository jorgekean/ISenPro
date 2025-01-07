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
PRIMARY KEY CLUSTERED 
(
	[ReferenceTableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
