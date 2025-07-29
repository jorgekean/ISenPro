CREATE TABLE [dbo].[PurchaseRequestItems] (
    [PurchaseRequestItemsId] INT             IDENTITY (1, 1) NOT NULL,
    [PurchaseRequestId]      INT             NOT NULL,
    [CatalogueId]            INT             NULL,
    [ItemDescription]        NVARCHAR (MAX)  NULL,
    [UnitOfMeasurement]      INT             NULL,
    [RequestedQuantity]      INT             NULL,
    [UnitPrice]              DECIMAL (18, 2) NULL,
    [Amount]                 DECIMAL (18, 2) NULL,
    [RequestingOfficeId]     INT             NOT NULL,
    [AvailableAt]            INT             NULL,
    [ItemType]               INT             NULL,
    [IsActive]               BIT             NOT NULL,
    [CreatedByUserId]        INT             NOT NULL,
    [CreatedDate]            DATETIME        NOT NULL,
    [UpdatedByUserId]        INT             NULL,
    [UpdatedDate]            DATETIME        NULL,
    [DeletedByUserId]        INT             NULL,
    [DeletedDate]            DATETIME        NULL,
    [IsFailed]               BIT             NOT NULL,
    [AmendedQuantity]        INT             NULL,
    [AmendedUnitPrice]       DECIMAL (18, 2) NULL,
    CONSTRAINT [PK__Purchase__2F2273C908869DF7] PRIMARY KEY CLUSTERED ([PurchaseRequestItemsId] ASC),
    CONSTRAINT [FK1D97D78EB89E1275] FOREIGN KEY ([PurchaseRequestId]) REFERENCES [dbo].[PurchaseRequests] ([PurchaseRequestId])
);



