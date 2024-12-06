CREATE TABLE [dbo].[SS_Suppliers](
	[SupplierId] [int] IDENTITY(1,1) NOT NULL,
	[Blacklist] [bit] NOT NULL,
	[CompanyName] [nvarchar](200) NULL,
	[Address] [nvarchar](500) NULL,
	[EmailAddress] [nvarchar](100) NULL,
	[Remarks] [nvarchar](max) NULL,
	[FaxNumber] [nvarchar](50) NULL,
	[Tin] [nvarchar](30) NULL,
	[Industry] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedByUserId] INT NOT NULL,
	[CreatedDate] [datetime] NOT NULL
PRIMARY KEY CLUSTERED 
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
