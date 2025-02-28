
CREATE VIEW dbo.v_PSDBMCatalogue AS

SELECT i.Name ItemType,
       a.Code ItemCode,
       cat.Name Productcategory,
       acc.Code AccountCode,
       um.Code UnitOfMeasurement,
       a.Description,
       a.UnitPrice
FROM dbo.SS_PSDBMCatalogue a
    JOIN dbo.SS_ItemType i
        ON i.ItemTypeId = a.ItemTypeId
    JOIN dbo.SS_MajorCategory cat
        ON cat.MajorCategoryId = a.MajorCategoryId
    JOIN dbo.SS_AccountCode acc
        ON acc.AccountCodeId = a.AccountCodeId
    JOIN dbo.SS_UnitOfMeasurement um
        ON um.UnitOfMeasurementId = a.UnitOfMeasurementId;