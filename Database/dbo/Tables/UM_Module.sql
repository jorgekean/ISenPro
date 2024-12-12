CREATE TABLE [dbo].[UM_Module] (
    [ModuleId]        INT            IDENTITY (1, 1) NOT NULL,
    [Code]            NVARCHAR (50)  NULL,
    [Name]            NVARCHAR (100) NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [ParentModuleId]  INT            NOT NULL,
    [PageId]          INT            NOT NULL,
    [IsActive]        BIT            NOT NULL,
    [CreatedByUserId] INT            NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    CONSTRAINT [PK_UM_Module] PRIMARY KEY CLUSTERED ([ModuleId] ASC),
    CONSTRAINT [FK_UM_Module_UM_Page] FOREIGN KEY ([PageId]) REFERENCES [dbo].[UM_Page] ([PageId])
);

