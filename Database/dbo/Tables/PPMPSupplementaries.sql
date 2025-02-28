CREATE TABLE [dbo].[PPMPSupplementaries] (
    [PPMPSupplementaryId] INT             IDENTITY (1, 1) NOT NULL,
    [PPMPId]              INT             NOT NULL,
    [Description]         NVARCHAR (MAX)  NULL,
    [FirstQuarter]        INT             NOT NULL,
    [SecondQuarter]       INT             NOT NULL,
    [ThirdQuarter]        INT             NOT NULL,
    [FourthQuarter]       INT             NOT NULL,
    [UnitPrice]           DECIMAL (19, 5) NOT NULL,
    [Amount]              DECIMAL (19, 5) NOT NULL,
    [Remarks]             NVARCHAR (MAX)  NULL,
    [IsActive]            BIT             NOT NULL,
    [CreatedByUserId]     INT             NOT NULL,
    [CreatedDate]         DATETIME        NOT NULL,
    [UpdatedByUserId]     INT             NULL,
    [UpdatedDate]         DATETIME        NULL,
    [DeletedByUserId]     INT             NULL,
    [DeletedDate]         DATETIME        NULL,
    [SupplementaryId]     INT             NOT NULL,
    CONSTRAINT [PK__PPMPSupp__6D54EC755C73118F] PRIMARY KEY CLUSTERED ([PPMPSupplementaryId] ASC),
    CONSTRAINT [FKF25AB4FE366CDA2C] FOREIGN KEY ([SupplementaryId]) REFERENCES [dbo].[SS_SupplementaryCatalogue] ([SupplementaryCatalogueId]),
    CONSTRAINT [FKF25AB4FE597D4FBB] FOREIGN KEY ([PPMPId]) REFERENCES [dbo].[PPMPs] ([PPMPId])
);





