CREATE VIEW dbo.v_PR_PpmpPSDBMCatalogues AS
SELECT 
    ppmp.RequestingOfficeId,
	ppmp.BudgetYear,
    pc.PPMPCatalogueId,
    pc.PPMPId,
    pc.Description,
    pc.FirstQuarter FirstQty,
    pc.SecondQuarter SecondQty,
    pc.ThirdQuarter ThirdQty,
    pc.FourthQuarter FourthQty,
    pc.UnitPrice,
    pc.Amount,
    pc.Remarks,
    pc.IsActive,
    pc.CatalogueId,
    c.Code AS CatalogueCode,
    mc.Name AS MajorCategoryName,
	uom.UnitOfMeasurementId,
    uom.Code AS UnitOfMeasurementCode,
	it.Name ItemName,	
	it.ItemTypeId ItemTypeId,
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
JOIN dbo.PPMPs ppmp ON ppmp.PPMPId = pc.PPMPId AND ppmp.Status = 'approved' AND ppmp.IsActive=1
JOIN dbo.SS_ItemType it ON it.ItemTypeId = ac.ItemTypeId