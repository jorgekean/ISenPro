CREATE TABLE [dbo].[SS_MajorCategory] (
    [MajorCategoryId] INT            IDENTITY (1, 1) NOT NULL,
    [Code]            NVARCHAR (100) NOT NULL,
    [Name]            NVARCHAR (200) NOT NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [IsActive]        BIT            NOT NULL,
    [CreatedByUserId] INT            NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [AccountCodeId]   INT            NULL,
    CONSTRAINT [PK_SS_MajorCategory] PRIMARY KEY CLUSTERED ([MajorCategoryId] ASC),
    CONSTRAINT [FK_SS_MajorCategory_SS_AccountCode] FOREIGN KEY ([AccountCodeId]) REFERENCES [dbo].[SS_AccountCode] ([AccountCodeId])
);

