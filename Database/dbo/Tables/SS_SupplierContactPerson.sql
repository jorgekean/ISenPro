CREATE TABLE [dbo].[SS_SupplierContactPerson] (
    [SupplierContactPersonId] [int] IDENTITY(1,1) NOT NULL,
	[ContactPerson] [nvarchar](max) NULL,
	[ContactNumber] [nvarchar](max) NULL,
	[IsActive] [bit] NULL,
	[CreatedByUserId] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
	[SupplierId] [int] NULL,

    CONSTRAINT [PK_SS_SupplierContactPerson] PRIMARY KEY CLUSTERED ([SupplierContactPersonId] ASC),
	CONSTRAINT [FK_SS_SupplierContactPerson_SS_Suppliers] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[SS_Suppliers] ([SupplierId])
);