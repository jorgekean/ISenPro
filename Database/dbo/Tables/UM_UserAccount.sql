CREATE TABLE [dbo].[UM_UserAccount] (
    [UserAccountId]   INT            IDENTITY (1, 1) NOT NULL,
    [UserID]          NVARCHAR (50)  NOT NULL,
    [Password]        NVARCHAR (100) NOT NULL,
    [ExpireDate]      DATETIME       NULL,
    [IsAdmin]         BIT            NOT NULL,
    [IsActive]        BIT            NOT NULL,
    [CreatedByUserId] INT            NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [PersonId]        INT            NOT NULL,
    [RoleId]          INT            NULL,
    CONSTRAINT [PK_UM_UserAccount_1] PRIMARY KEY CLUSTERED ([UserAccountId] ASC),
    CONSTRAINT [FK_UM_UserAccount_UM_Person] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[UM_Person] ([PersonId]),
    CONSTRAINT [FK_UM_UserAccount_UM_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[UM_Role] ([RoleId]),
    CONSTRAINT [IX_UM_UserAccount] UNIQUE NONCLUSTERED ([PersonId] ASC)
);





