CREATE TABLE [dbo].[PPMPCatalogues] (
    [PPMPCatalogueId] INT             IDENTITY (1, 1) NOT NULL,
    [PPMPId]          INT             NOT NULL,
    [Description]     NVARCHAR (MAX)  NULL,
    [FirstQuarter]    INT             NOT NULL,
    [SecondQuarter]   INT             NOT NULL,
    [ThirdQuarter]    INT             NOT NULL,
    [FourthQuarter]   INT             NOT NULL,
    [UnitPrice]       DECIMAL (19, 5) NOT NULL,
    [Amount]          DECIMAL (19, 5) NOT NULL,
    [Remarks]         NVARCHAR (MAX)  NULL,
    [IsActive]        BIT             NOT NULL,
    [CreatedByUserId] INT             NOT NULL,
    [CreatedDate]     DATETIME        NOT NULL,
    [UpdatedByUserId] INT             NOT NULL,
    [UpdatedDate]     DATETIME        NOT NULL,
    [DeletedByUserId] INT             NOT NULL,
    [DeletedDate]     DATETIME        NOT NULL,
    [CatalogueId]     INT             NOT NULL,
    CONSTRAINT [PK__PPMPCata__5036F51751015EE3] PRIMARY KEY CLUSTERED ([PPMPCatalogueId] ASC),
    CONSTRAINT [FKCCA15ADD597D4FBB] FOREIGN KEY ([PPMPId]) REFERENCES [dbo].[PPMPs] ([PPMPId]),
    CONSTRAINT [FKCCA15ADD6686F299] FOREIGN KEY ([CatalogueId]) REFERENCES [dbo].[SS_PSDBMCatalogue] ([PSDBMCatalogueId])
);



