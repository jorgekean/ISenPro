CREATE VIEW v_PpmpPSDBMCatalogues AS
SELECT 
    pc.*,
    c.Code AS CatalogueCode,
    mc.Name AS MajorCategoryName,
    uom.Code AS UnitOfMeasurementCode,
    ac.Description AS AccountCodeDescription
FROM dbo.PPMPCatalogues pc
LEFT JOIN dbo.SS_PSDBMCatalogue c
    ON pc.CatalogueId = c.PSDBMCatalogueId
LEFT JOIN dbo.SS_MajorCategory mc
    ON c.MajorCategoryId = mc.MajorCategoryId
LEFT JOIN dbo.SS_UnitOfMeasurement uom
    ON c.UnitOfMeasurementId = uom.UnitOfMeasurementId
LEFT JOIN dbo.SS_AccountCode ac
    ON c.AccountCodeId = ac.AccountCodeId;