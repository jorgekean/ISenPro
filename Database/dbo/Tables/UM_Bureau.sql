CREATE TABLE [dbo].[UM_Bureau] (
    [BureauId]        INT            IDENTITY (1, 1) NOT NULL,
    [Code]            NVARCHAR (100) NOT NULL,
    [Name]            NVARCHAR (200) NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [IsActive]        BIT            NOT NULL,
    [CreatedByUserId] BIGINT         NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [DivisionId]      INT            NULL,
    [GroupId]         INT            NULL,
    CONSTRAINT [PK_UM_Bureau] PRIMARY KEY CLUSTERED ([BureauId] ASC),
    CONSTRAINT [FK_UM_Bureau_UM_Division] FOREIGN KEY ([DivisionId]) REFERENCES [dbo].[UM_Division] ([DivisionId])
);

