CREATE TABLE [dbo].[CT_UM_Role_Details] (
    [ChangeID]  INT            NOT NULL,
    [FieldName] VARCHAR (128)  NOT NULL,
    [OldValue]  NVARCHAR (MAX) NULL,
    [NewValue]  NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([ChangeID] ASC, [FieldName] ASC),
    FOREIGN KEY ([ChangeID]) REFERENCES [dbo].[CT_UM_Role] ([ChangeID])
);

