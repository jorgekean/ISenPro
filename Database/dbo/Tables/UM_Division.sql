CREATE TABLE [dbo].[UM_Division] (
    [DivisionId]      INT            IDENTITY (1, 1) NOT NULL,
    [Code]            NVARCHAR (100) NOT NULL,
    [Name]            NVARCHAR (200) NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [IsActive]        BIT            NOT NULL,
    [CreatedByUserId] INT            NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    CONSTRAINT [PK_UM_Division] PRIMARY KEY CLUSTERED ([DivisionId] ASC)
);



