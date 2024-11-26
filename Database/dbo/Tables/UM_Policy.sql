CREATE TABLE [dbo].[UM_Policy] (
    [PolicyId]        INT            IDENTITY (1, 1) NOT NULL,
    [Code]            NVARCHAR (50)  NOT NULL,
    [Name]            NVARCHAR (200) NOT NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [IsActive]        BIT            NOT NULL,
    [CreatedByUserId] INT            NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    CONSTRAINT [PK_UM_Policy] PRIMARY KEY CLUSTERED ([PolicyId] ASC)
);

