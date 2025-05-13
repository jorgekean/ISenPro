CREATE TABLE [dbo].[APPDetails] (
    [APPDetailId]     INT      IDENTITY (1, 1) NOT NULL,
    [AppId]           INT      NOT NULL,
    [IsActive]        BIT      NOT NULL,
    [CreatedByUserId] INT      NOT NULL,
    [CreatedDate]     DATETIME NOT NULL,
    [UpdatedByUserId] INT      NULL,
    [UpdatedDate]     DATETIME NULL,
    [DeletedByUserId] INT      NULL,
    [DeletedDate]     DATETIME NULL,
    [PpmpId]          INT      NOT NULL,
    CONSTRAINT [PK__APPDetai__EB87DD4A364D68A7] PRIMARY KEY CLUSTERED ([APPDetailId] ASC),
    CONSTRAINT [FK_APPDetails_APPs] FOREIGN KEY ([AppId]) REFERENCES [dbo].[APPs] ([APPId])
);

