
CREATE VIEW dbo.v_SupplierIndex AS
 SELECT 
	a.SupplierId as Id,
	a.CompanyName,
	b.Code as IndustryName,
	a.EmailAddress,
	a.[Address],
	case when a.Blacklist = 1 then 'Yes' else 'No' end as IsBlackListedStr,
	a.IsActive,
	a.CreatedDate
FROM dbo.SS_Suppliers a
    LEFT JOIN dbo.SS_ReferenceTable b
        ON a.Industry = b.ReferenceTableId