
CREATE TABLE [dbo].[SS_PSDBMCatalogueOffice](
	[PSDBMCatalogueOfficeId] [int] IDENTITY(1,1) NOT NULL,
	[PSDBMCatalogueId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedByUserId] INT NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	CONSTRAINT [PK_SS_PSDBMCatalogueOffice] PRIMARY KEY CLUSTERED ([PSDBMCatalogueOfficeId] ASC),
	CONSTRAINT [FK_SS_PSDBMCatalogueOffice_SS_PSDBMCatalogue] FOREIGN KEY ([PSDBMCatalogueId]) REFERENCES [dbo].[SS_PSDBMCatalogue] ([PSDBMCatalogueId]),
	CONSTRAINT [FK_SS_PSDBMCatalogueOffice_UM_Department] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[UM_Department] ([DepartmentId])
);