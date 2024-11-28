CREATE TABLE [dbo].[SS_AccountCode] (
    [AccountCodeId]   INT            IDENTITY (1, 1) NOT NULL,
    [Code]            NVARCHAR (100) NOT NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [IsActive]        BIT            NOT NULL,
    [CreatedByUserId] INT            NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [ItemTypeId]      INT            NULL,
    CONSTRAINT [PK_SS_AccountCode] PRIMARY KEY CLUSTERED ([AccountCodeId] ASC),
    CONSTRAINT [FK_SS_AccountCode_SS_ItemType] FOREIGN KEY ([ItemTypeId]) REFERENCES [dbo].[SS_ItemType] ([ItemTypeId])
);

