
CREATE VIEW dbo.v_PSDBMCatalogueIndex AS
 SELECT 
	a.PSDBMCatalogueId as Id,
	a.Code,
	a.[Description],
	a.UnitPrice,
	b.[Name] as UnitOfMeasurementCode,
	i.[Name] as ItemTypeName,
	d.[Name] as MajorCategoryName,
	case when e.[Name] is null then '' else e.[Name] end as SubCategoryName,
	a.IsActive,
	a.CreatedDate
FROM dbo.SS_PSDBMCatalogue a
    LEFT JOIN dbo.SS_UnitOfMeasurement b
        ON a.UnitOfMeasurementId= b.UnitOfMeasurementId
    LEFT JOIN dbo.SS_ItemType c
        ON c.ItemTypeId= a.ItemTypeId
	LEFT JOIN dbo.SS_MajorCategory d
        ON d.MajorCategoryId = a.MajorCategoryId
    LEFT JOIN dbo.SS_SubCategory e
        ON e.SubCategoryId = a.SubCategoryId
	LEFT JOIN dbo.SS_ItemType i
        ON i.ItemTypeId = a.ItemTypeId
where a.IsCurrent = 1
