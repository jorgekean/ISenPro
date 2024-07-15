CREATE TABLE [dbo].[ChangeDetailsTable] (
    [ChangeID]  INT            NOT NULL,
    [FieldName] VARCHAR (128)  NOT NULL,
    [OldValue]  NVARCHAR (MAX) NULL,
    [NewValue]  NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([ChangeID] ASC, [FieldName] ASC),
    FOREIGN KEY ([ChangeID]) REFERENCES [dbo].[ChangeTrackingTable] ([ChangeID])
);

