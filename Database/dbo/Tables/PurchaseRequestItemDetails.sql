CREATE TABLE [dbo].[PurchaseRequestItemDetails] (
    [PurchaseRequestItemDetailsId] INT             IDENTITY (1, 1) NOT NULL,
    [PurchaseRequestItemsId]       INT             NOT NULL,
    [ItemSpecification]            NVARCHAR (MAX)  NULL,
    [RequestedQuantity]            INT             NULL,
    [UnitPrice]                    DECIMAL (18, 2) NULL,
    [ItemType]                     INT             NULL,
    [IsActive]                     BIT             NOT NULL,
    [CreatedByUserId]              INT             NOT NULL,
    [CreatedDate]                  DATETIME        NOT NULL,
    [UpdatedByUserId]              INT             NULL,
    [UpdatedDate]                  DATETIME        NULL,
    [DeletedByUserId]              INT             NULL,
    [DeletedDate]                  DATETIME        NULL,
    [UnitOfMeasure]                INT             NULL,
    CONSTRAINT [PK__Purchase__A62D008304B60D13] PRIMARY KEY CLUSTERED ([PurchaseRequestItemDetailsId] ASC),
    CONSTRAINT [FK544D1A99F79ECDBD] FOREIGN KEY ([PurchaseRequestItemsId]) REFERENCES [dbo].[PurchaseRequestItems] ([PurchaseRequestItemsId])
);



