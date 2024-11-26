CREATE TABLE [dbo].[UM_UserAccount] (
    [UserAccountId]   INT            IDENTITY (1, 1) NOT NULL,
    [UserID]          NVARCHAR (50)  NOT NULL,
    [Password]        NVARCHAR (100) NOT NULL,
    [ExpireDate]      DATETIME       NULL,
    [IsAdmin]         BIT            NOT NULL,
    [IsActive]        BIT            NOT NULL,
    [CreatedByUserId] BIGINT         NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [PersonId]        INT            NOT NULL,
    [RoleId]          INT            NULL,
    CONSTRAINT [PK_UM_UserAccount] PRIMARY KEY CLUSTERED ([UserAccountId] ASC)
);

