Delete from [dbo].[SS_ItemStatus]

SET IDENTITY_INSERT [dbo].[SS_ItemStatus] ON

INSERT INTO [dbo].[SS_ItemStatus] ([ItemStatusId], [Code], [Name], [Description], [IsActive], [CreatedByUserId], [CreatedDate]) VALUES (1, N'DISPOSAL', N'DISPOSAL', N'DISPOSAL', 1, 1, N'2014-01-29 16:57:22')
INSERT INTO [dbo].[SS_ItemStatus] ([ItemStatusId], [Code], [Name], [Description], [IsActive], [CreatedByUserId], [CreatedDate]) VALUES (2, N'ISSUED', N'ISSUED', N'ISSUED', 1, 1, N'2014-01-29 16:57:22')
INSERT INTO [dbo].[SS_ItemStatus] ([ItemStatusId], [Code], [Name], [Description], [IsActive], [CreatedByUserId], [CreatedDate]) VALUES (3, N'NEW', N'NEW', N'NEW', 1, 1, N'2014-01-29 16:57:22')
INSERT INTO [dbo].[SS_ItemStatus] ([ItemStatusId], [Code], [Name], [Description], [IsActive], [CreatedByUserId], [CreatedDate]) VALUES (4, N'RETURNED', N'RETURNED', N'RETURNED', 1, 1, N'2014-01-29 16:57:22')
INSERT INTO [dbo].[SS_ItemStatus] ([ItemStatusId], [Code], [Name], [Description], [IsActive], [CreatedByUserId], [CreatedDate]) VALUES (5, N'RE-ISSUED', N'RE-ISSUED', N'RE-ISSUED', 1, 1, N'2014-03-03 15:53:47')
INSERT INTO [dbo].[SS_ItemStatus] ([ItemStatusId], [Code], [Name], [Description], [IsActive], [CreatedByUserId], [CreatedDate]) VALUES (6, N'REMOVED', N'REMOVED PER AOM', N'REMOVED PER AOM', 1, 155, N'2021-08-04 12:55:34')

SET IDENTITY_INSERT [dbo].[SS_ItemStatus] OFF