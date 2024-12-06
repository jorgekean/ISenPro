CREATE TABLE [dbo].[UM_Section] (
    [SectionId]       INT            IDENTITY (1, 1) NOT NULL,
    [Code]            NVARCHAR (100) NOT NULL,
    [Name]            NVARCHAR (200) NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [IsActive]        BIT            NOT NULL,
    [CreatedByUserId] INT            NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [DepartmentId]    INT            NULL,
    CONSTRAINT [PK_UM_Section] PRIMARY KEY CLUSTERED ([SectionId] ASC),
    CONSTRAINT [FK_UM_Section_UM_Department] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[UM_Department] ([DepartmentId])
);



