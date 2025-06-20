﻿
CREATE VIEW dbo.v_app_PpmpCatalogues
AS
SELECT A.BudgetYear,
       d.Name RequestingOffice,
       spc.Code ItemCode,
       mc.NAME ProductCategory,
       um.NAME UnitOfMeasure,
       pc.*
FROM dbo.PPMPs A
    JOIN dbo.PPMPCatalogues pc
        ON pc.PPMPId = A.PPMPId
    JOIN dbo.UM_Department d
        ON d.DepartmentId = A.RequestingOfficeId
    JOIN dbo.SS_PSDBMCatalogue spc
        ON spc.PSDBMCatalogueId = pc.CatalogueId
    JOIN dbo.SS_MajorCategory mc
        ON mc.MajorCategoryId = spc.MajorCategoryId
    JOIN dbo.SS_UnitOfMeasurement um
        ON um.UnitOfMeasurementId = spc.UnitOfMeasurementId
WHERE A.IsActive = 1
      AND pc.IsActive = 1;