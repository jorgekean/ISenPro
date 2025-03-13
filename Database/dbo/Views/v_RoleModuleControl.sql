
CREATE VIEW [dbo].[v_RoleModuleControl]
AS
SELECT DISTINCT
       mc.*, m.Name ModuleName, pm.Description ParentModuleName
FROM dbo.UM_ModuleControl mc
    INNER JOIN dbo.UM_PolicyModuleControls pmc
        ON mc.ModuleControlId = pmc.ModuleControlId
    INNER JOIN dbo.UM_PolicyRoles pr
        ON pmc.PolicyId = pr.PolicyId
JOIN dbo.UM_Module m ON m.ModuleId = mc.ModuleId AND m.IsActive=1
   JOIN dbo.v_ParentModule pm ON pm.ParentModuleId = m.ParentModuleId 
 
WHERE pr.IsActive = 1
      AND pmc.IsChecked = 1
      AND pmc.IsActive = 1
      AND pr.IsActive = 1
      AND mc.IsChecked = 1 AND mc.IsActive=1
      --AND pr.RoleId = @roleId