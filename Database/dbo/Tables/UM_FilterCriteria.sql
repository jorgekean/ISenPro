CREATE TABLE [dbo].[UM_FilterCriteria] (
    [FilterCriteriaId] INT      IDENTITY (1, 1) NOT NULL,
    [FilterBased]      INT      NOT NULL,
    [IsActive]         BIT      NOT NULL,
    [CreatedByUserId]  INT      NOT NULL,
    [CreatedDate]      DATETIME NOT NULL,
    [RoleId]           INT      NULL,
    [UserAccountId]    INT      NULL,
    [ParentModule]     INT      NULL,
    CONSTRAINT [PK__FilterCr__E6D3405B5E169ED5] PRIMARY KEY CLUSTERED ([FilterCriteriaId] ASC),
    CONSTRAINT [FK2D7383BA419B0845] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[UM_Role] ([RoleId]),
    CONSTRAINT [FK2D7383BA681C74F9] FOREIGN KEY ([UserAccountId]) REFERENCES [dbo].[UM_UserAccount] ([UserAccountId])
);

