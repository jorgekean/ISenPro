﻿
CREATE TABLE [dbo].[SS_PSDBMCatalogue](
	[PSDBMCatalogueId] [int] IDENTITY(1,1) NOT NULL,
	[CatalogueYear] [datetime] NULL,
	[Code] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[UnitPrice] [decimal](19, 5) NULL,
	[IsCurrent] [bit] NULL,
	[Remarks] [nvarchar](max) NULL,
	[Thumbnail] [nvarchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedByUserId] INT NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UnitOfMeasurementId] [int] NOT NULL,
	[IsOriginal] [bit] NULL,
	CONSTRAINT [PK_SS_PSDBMCatalogue] PRIMARY KEY CLUSTERED ([PSDBMCatalogueId] ASC),
	CONSTRAINT [FK_SS_PSDBMCatalogue_SS_UnitOfMeasurement] FOREIGN KEY ([UnitOfMeasurementId]) REFERENCES [dbo].[SS_UnitOfMeasurement] ([UnitOfMeasurementId])
);