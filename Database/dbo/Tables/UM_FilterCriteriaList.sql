CREATE TABLE [dbo].[UM_FilterCriteriaList] (
    [FilterCriteriaListId] INT      IDENTITY (1, 1) NOT NULL,
    [FilterCriteriaId]     INT      NOT NULL,
    [CriteriaId]           INT      NOT NULL,
    [IsActive]             BIT      NOT NULL,
    [CreatedByUserId]      INT      NOT NULL,
    [CreatedDate]          DATETIME NOT NULL,
    CONSTRAINT [PK__FilterCr__E906902169885181] PRIMARY KEY CLUSTERED ([FilterCriteriaListId] ASC),
    CONSTRAINT [FKD031074A630A3B93] FOREIGN KEY ([FilterCriteriaId]) REFERENCES [dbo].[UM_FilterCriteria] ([FilterCriteriaId])
);

