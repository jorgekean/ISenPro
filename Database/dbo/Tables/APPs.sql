CREATE TABLE [dbo].[APPs] (
    [APPId]                       INT             IDENTITY (1, 1) NOT NULL,
    [APPNo]                       NVARCHAR (100)  NULL,
    [BudgetYear]                  SMALLINT        NOT NULL,
    [TotalAmount]                 DECIMAL (19, 5) NULL,
    [AdditionalInflationValue]    DECIMAL (19, 5) NULL,
    [AdditionalTenPercent]        DECIMAL (19, 5) NULL,
    [GrandTotal]                  DECIMAL (19, 5) NULL,
    [Status]                      VARCHAR (50)    NOT NULL,
    [AmendedTotalAmount]          DECIMAL (19, 5) NULL,
    [AmendedAdditionalTenPercent] DECIMAL (19, 5) NULL,
    [AmendedGrandTotal]           DECIMAL (19, 5) NULL,
    [IsSubmitted]                 BIT             NOT NULL,
    [SubmittedByUserId]           INT             NULL,
    [SubmittedDate]               DATETIME        NULL,
    [IsActive]                    BIT             NOT NULL,
    [CreatedByUserId]             INT             NOT NULL,
    [CreatedDate]                 DATETIME        NOT NULL,
    [UpdatedByUserId]             INT             NULL,
    [UpdatedDate]                 DATETIME        NULL,
    [DeletedByUserId]             INT             NULL,
    [DeletedDate]                 DATETIME        NULL,
    CONSTRAINT [PK__APPs__AA3B3831327CD7C3] PRIMARY KEY CLUSTERED ([APPId] ASC)
);



