CREATE TABLE [dbo].[UM_ModuleControl] (
    [ModuleControlId] INT      IDENTITY (1, 1) NOT NULL,
    [IsChecked]       BIT      NOT NULL,
    [ControlId]       INT      NOT NULL,
    [ModuleId]        INT      NOT NULL,
    [IsActive]        BIT      NOT NULL,
    [CreatedByUserId] INT      NOT NULL,
    [CreatedDate]     DATETIME NOT NULL,
    CONSTRAINT [PK_UM_ModuleControl] PRIMARY KEY CLUSTERED ([ModuleControlId] ASC),
    CONSTRAINT [FK_UM_ModuleControl_UM_Control] FOREIGN KEY ([ControlId]) REFERENCES [dbo].[UM_Control] ([ControlId]),
    CONSTRAINT [FK_UM_ModuleControl_UM_Module] FOREIGN KEY ([ModuleId]) REFERENCES [dbo].[UM_Module] ([ModuleId])
);

