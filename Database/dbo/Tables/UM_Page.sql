CREATE TABLE [dbo].[UM_Page] (
    [PageId]      INT            NOT NULL,
    [PageName]    NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (200) NULL,
    CONSTRAINT [PK_UM_Page] PRIMARY KEY CLUSTERED ([PageId] ASC)
);

