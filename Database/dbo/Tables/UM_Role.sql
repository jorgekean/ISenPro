CREATE TABLE [dbo].[UM_Role] (
    [RoleId]      INT           IDENTITY (1, 1) NOT NULL,
    [Code]        VARCHAR (20)  NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (200) NULL,
    [IsActive]    BIT           CONSTRAINT [DF_UM_Role_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate] DATETIME      NOT NULL,
    [CreatedBy]   INT           NOT NULL,
    CONSTRAINT [PK_UM_Role] PRIMARY KEY CLUSTERED ([RoleId] ASC)
);






GO
