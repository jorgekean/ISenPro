DELETE from dbo.[SS_PurchasingType]

SET IDENTITY_INSERT [dbo].[SS_PurchasingType] ON

INSERT INTO [dbo].[SS_PurchasingType] ([PurchasingTypeId], [Code], [Name], [Description], [WithCondition], [MinimumAmount], [MaximumAmount], [IsActive], [CreatedByUserId], [CreatedDate]) VALUES (1, N'001', N'PETTY CASH', N'PETTY CASH', 1, 1, 15000, 1, 1, N'2014-03-01 18:33:52')
INSERT INTO [dbo].[SS_PurchasingType] ([PurchasingTypeId], [Code], [Name], [Description], [WithCondition], [MinimumAmount], [MaximumAmount], [IsActive], [CreatedByUserId], [CreatedDate]) VALUES (2, N'002', N'EMERGENCY/OTHER PURCHASE', N'EMERGENCY/OTHER PURCHASE', 1, 1, 99999999, 1, 1, N'2014-03-01 18:34:31')
INSERT INTO [dbo].[SS_PurchasingType] ([PurchasingTypeId], [Code], [Name], [Description], [WithCondition], [MinimumAmount], [MaximumAmount], [IsActive], [CreatedByUserId], [CreatedDate]) VALUES (3, N'003', N'BUDGETED', N'BUDGETED', 0, 0, 0, 1, 1, N'2014-03-01 18:34:56')

SET IDENTITY_INSERT [dbo].[SS_PurchasingType] OFF