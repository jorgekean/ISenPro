CREATE TABLE [dbo].[SS_UnitOfMeasurement] (
    [UnitOfMeasurementId] INT            IDENTITY (1, 1) NOT NULL,
    [Code]                NVARCHAR (100) NULL,
    [Name]                NVARCHAR (200) NULL,
    [IsActive]            BIT            NULL,
    [CreatedByUserId]     INT            NOT NULL,
    [CreatedDate]         DATETIME       NOT NULL,
    CONSTRAINT [PK_SS_UnitOfMeasurement] PRIMARY KEY CLUSTERED ([UnitOfMeasurementId] ASC)
);

