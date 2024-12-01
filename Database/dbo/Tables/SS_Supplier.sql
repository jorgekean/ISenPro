CREATE TABLE [dbo].[SS_Suppliers](
	[SupplierId] [int] IDENTITY(1,1) NOT NULL,
	[Blacklist] [bit] NULL,
	[CompanyName] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[EmailAddress] [nvarchar](max) NULL,
	[Remarks] [nvarchar](max) NULL,
	[FaxNumber] [nvarchar](max) NULL,
	[Tin] [nvarchar](max) NULL,
	[Industry] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedByUserId] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedByUserId] [bigint] NULL,
	[UpdatedDate] [datetime] NULL,
	[RestoredByUserId] [bigint] NULL,
	[RestoredDate] [datetime] NULL,
	[DeletedByUserId] [bigint] NULL,
	[DeletedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
