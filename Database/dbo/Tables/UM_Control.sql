CREATE TABLE [dbo].[UM_Control] (
    [ControlId]   INT            NOT NULL,
    [ControlName] NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (200) NULL,
    CONSTRAINT [PK_UM_Control] PRIMARY KEY CLUSTERED ([ControlId] ASC)
);

