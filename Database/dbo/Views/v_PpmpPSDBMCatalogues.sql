
CREATE VIEW v_PpmpPSDBMCatalogues AS
SELECT 
    pc.*,
    c.Code AS CatalogueCode,
    mc.Name AS MajorCategoryName,
    uom.Code AS UnitOfMeasurementCode,
    ac.Description AS AccountCodeDescription
FROM dbo.PPMPCatalogues pc
JOIN dbo.SS_PSDBMCatalogue c
    ON pc.CatalogueId = c.PSDBMCatalogueId
JOIN dbo.SS_MajorCategory mc
    ON c.MajorCategoryId = mc.MajorCategoryId
JOIN dbo.SS_UnitOfMeasurement uom
    ON c.UnitOfMeasurementId = uom.UnitOfMeasurementId
JOIN dbo.SS_AccountCode ac
    ON c.AccountCodeId = ac.AccountCodeId;