CREATE TABLE [dbo].[SS_SubCategory] (
    [SubCategoryId]   INT            IDENTITY (1, 1) NOT NULL,
    [Code]            NVARCHAR (100) NOT NULL,
    [Name]            NVARCHAR (200) NOT NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [IsActive]        BIT            NOT NULL,
    [CreatedByUserId] INT            NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [MajorCategoryId] INT            NULL,
    CONSTRAINT [PK_SS_SubCategory] PRIMARY KEY CLUSTERED ([SubCategoryId] ASC),
    CONSTRAINT [FK_SS_SubCategory_SS_MajorCategory] FOREIGN KEY ([MajorCategoryId]) REFERENCES [dbo].[SS_MajorCategory] ([MajorCategoryId])
);

