CREATE TABLE [dbo].[SS_ItemType] (
    [ItemTypeId]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (MAX) NOT NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [IsActive]        BIT            NOT NULL,
    [CreatedByUserId] INT            NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    CONSTRAINT [PK_SS_ItemType] PRIMARY KEY CLUSTERED ([ItemTypeId] ASC)
);

