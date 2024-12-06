CREATE TABLE [dbo].[UM_Department] (
    [DepartmentId]         INT            IDENTITY (1, 1) NOT NULL,
    [Code]                 NVARCHAR (100) NOT NULL,
    [Name]                 NVARCHAR (200) NULL,
    [Description]          NVARCHAR (MAX) NULL,
    [ResponsibilityCenter] NVARCHAR (MAX) NULL,
    [IsActive]             BIT            NOT NULL,
    [CreatedByUserId]      BIGINT         NOT NULL,
    [CreatedDate]          DATETIME       NOT NULL,
    [BureauId]             INT            NULL,
    CONSTRAINT [PK_UM_Department] PRIMARY KEY CLUSTERED ([DepartmentId] ASC),
    CONSTRAINT [FK_UM_Department_UM_Bureau] FOREIGN KEY ([BureauId]) REFERENCES [dbo].[UM_Bureau] ([BureauId])
);

