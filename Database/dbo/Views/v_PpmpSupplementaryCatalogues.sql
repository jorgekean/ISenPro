CREATE VIEW v_PpmpSupplementaryCatalogues AS
SELECT 
    pc.*,
    c.Code AS CatalogueCode,
    mc.Name AS MajorCategoryName,
    uom.Code AS UnitOfMeasurementCode,
    ac.Description AS AccountCodeDescription
FROM dbo.PPMPSupplementaries pc
 JOIN dbo.SS_SupplementaryCatalogue c
    ON pc.SupplementaryId = c.SupplementaryCatalogueId
 JOIN dbo.SS_MajorCategory mc
    ON c.MajorCategoryId = mc.MajorCategoryId
 JOIN dbo.SS_UnitOfMeasurement uom
    ON c.UnitOfMeasurementId = uom.UnitOfMeasurementId
 JOIN dbo.SS_AccountCode ac
    ON c.AccountCodeId = ac.AccountCodeId;