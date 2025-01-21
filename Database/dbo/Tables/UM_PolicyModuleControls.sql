CREATE TABLE [dbo].[UM_PolicyModuleControls] (
    [PolicyModuleControlId] INT      IDENTITY (1, 1) NOT NULL,
    [PolicyId]              INT      NOT NULL,
    [IsChecked]             BIT      NOT NULL,
    [IsActive]              BIT      NOT NULL,
    [CreatedByUserId]       INT      NOT NULL,
    [CreatedDate]           DATETIME NOT NULL,
    [ModuleControlId]       INT      NOT NULL,
    CONSTRAINT [PK_UM_PolicyModuleControls] PRIMARY KEY CLUSTERED ([PolicyModuleControlId] ASC),
    CONSTRAINT [FK_UM_PolicyModuleControls_UM_ModuleControl] FOREIGN KEY ([ModuleControlId]) REFERENCES [dbo].[UM_ModuleControl] ([ModuleControlId])
);



