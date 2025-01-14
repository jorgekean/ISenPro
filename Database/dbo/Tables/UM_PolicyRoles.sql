CREATE TABLE [dbo].[UM_PolicyRoles] (
    [PolicyRoleId]    INT      IDENTITY (1, 1) NOT NULL,
    [PolicyId]        INT      NOT NULL,
    [IsActive]        BIT      NOT NULL,
    [CreatedByUserId] INT      NOT NULL,
    [CreatedDate]     DATETIME NOT NULL,
    [RoleId]          INT      NOT NULL,
    CONSTRAINT [PK_UM_PolicyRoles] PRIMARY KEY CLUSTERED ([PolicyRoleId] ASC),
    CONSTRAINT [FK_UM_PolicyRoles_UM_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[UM_Role] ([RoleId])
);

