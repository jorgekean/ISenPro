CREATE TABLE [dbo].[PPMPs] (
    [PPMPId]                   INT             IDENTITY (1, 1) NOT NULL,
    [PPMPNo]                   NVARCHAR (100)  NOT NULL,
    [BudgetYear]               SMALLINT        NOT NULL,
    [Remarks]                  NVARCHAR (MAX)  NULL,
    [CatalogueAmount]          DECIMAL (19, 5) NULL,
    [SupplementaryAmount]      DECIMAL (19, 5) NULL,
    [ProjectAmount]            DECIMAL (19, 5) NULL,
    [TotalAmount]              DECIMAL (19, 5) NULL,
    [AdditionalInflationValue] DECIMAL (19, 5) NULL,
    [AdditionalTenPercent]     DECIMAL (19, 5) NULL,
    [GrandTotalAmount]         DECIMAL (19, 5) NULL,
    [IsSubmitted]              BIT             NOT NULL,
    [SubmittedByUserId]        INT             NULL,
    [SubmittedDate]            DATETIME        NULL,
    [IsActive]                 BIT             NOT NULL,
    [CreatedByUserId]          INT             NOT NULL,
    [CreatedDate]              DATETIME        NOT NULL,
    [UpdatedByUserId]          INT             NULL,
    [UpdatedDate]              DATETIME        NULL,
    [DeletedByUserId]          INT             NULL,
    [DeletedDate]              DATETIME        NULL,
    [RequestingOfficeId]       INT             NULL,
    [Status]                   VARCHAR (50)    NOT NULL,
    CONSTRAINT [PK__PPMPs__4DF9D7C73A1DF98B] PRIMARY KEY CLUSTERED ([PPMPId] ASC),
    CONSTRAINT [FK75F21864251B5D6C] FOREIGN KEY ([RequestingOfficeId]) REFERENCES [dbo].[UM_Department] ([DepartmentId])
);



