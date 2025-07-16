
CREATE VIEW dbo.v_PpmpPSDBMCatalogues AS
SELECT 
    ppmp.RequestingOfficeId,
	ppmp.BudgetYear,
    pc.*,
    c.Code AS CatalogueCode,
    mc.Name AS MajorCategoryName,
    uom.Code AS UnitOfMeasurementCode,
    ac.Description AS AccountCodeDescription
FROM dbo.PPMPCatalogues pc
JOIN dbo.SS_PSDBMCatalogue c
    ON pc.CatalogueId = c.PSDBMCatalogueId AND c.IsCurrent=1
JOIN dbo.SS_MajorCategory mc
    ON c.MajorCategoryId = mc.MajorCategoryId
JOIN dbo.SS_UnitOfMeasurement uom
    ON c.UnitOfMeasurementId = uom.UnitOfMeasurementId
JOIN dbo.SS_AccountCode ac
    ON c.AccountCodeId = ac.AccountCodeId
JOIN dbo.PPMPs ppmp ON ppmp.PPMPId = pc.PPMPId