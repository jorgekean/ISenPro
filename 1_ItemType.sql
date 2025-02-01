

DELETE from dbo.SS_ItemType

SET IDENTITY_INSERT dbo.SS_ItemType ON;

INSERT INTO dbo.SS_ItemType ([ItemTypeId], [Name], [Description], [IsActive], [CreatedByUserId], [CreatedDate])
VALUES
( 1, N'Equipment', N'Equipment', 1, 1, N'2014-02-03T00:00:00' ),
( 2, N'Supplies', N'Supplies', 1, 1, N'2014-02-03T00:00:00' )

SET IDENTITY_INSERT dbo.SS_ItemType OFF;