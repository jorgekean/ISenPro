CREATE TABLE [dbo].[SS_PurchasingType] (
    [PurchasingTypeId] INT             IDENTITY (1, 1) NOT NULL,
    [Code]             NVARCHAR (100)  NULL,
    [Name]             NVARCHAR (200)  NULL,
    [Description]      NVARCHAR (MAX)  NULL,
    [WithCondition]    BIT             NOT NULL,
    [MinimumAmount]    DECIMAL (18, 2) NULL,
    [MaximumAmount]    DECIMAL (18, 2) NULL,
    [IsActive]         BIT             NOT NULL,
    [CreatedByUserId]  INT             NOT NULL,
    [CreatedDate]      DATETIME        NOT NULL,
    CONSTRAINT [PK_SS_PurchasingType] PRIMARY KEY CLUSTERED ([PurchasingTypeId] ASC)
);

