﻿CREATE TABLE [dbo].[ChangeTrackingTable] (
    [ChangeID]  INT           IDENTITY (1, 1) NOT NULL,
    [Timestamp] DATETIME      NOT NULL,
    [Operation] VARCHAR (10)  NOT NULL,
    [UserID]    VARCHAR (128) NULL,
    PRIMARY KEY CLUSTERED ([ChangeID] ASC)
);

