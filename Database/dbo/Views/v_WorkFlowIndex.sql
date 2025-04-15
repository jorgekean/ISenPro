
CREATE VIEW dbo.v_WorkFlowIndex
AS
SELECT 
	w.WorkFlowId as Id,
    w.Code,
	w.[Name],
	w.[Description],
	m.[Name] as ModuleName,
    w.IsActive,
    w.CreatedDate,
    w.CreatedByUserId
FROM dbo.UM_WorkFlow w
    JOIN dbo.UM_Module m
        ON w.ModuleId = m.ModuleId