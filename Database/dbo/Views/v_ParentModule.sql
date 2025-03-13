
CREATE VIEW [dbo].[v_ParentModule]
AS
SELECT 
    11 AS ParentModuleId,
    'UserManagement' AS Name,
    'User Management' AS [Description]
UNION ALL
SELECT 
    12 AS ParentModuleId,
    'SystemSetup' AS Name,
    'System Setup' AS [Description]
UNION ALL
SELECT 
    13 AS ParentModuleId,
    'Transactions' AS Name,
    'Transactions' AS [Description]
UNION ALL
SELECT 
    14 AS ParentModuleId,
    'Reports' AS Name,
    'Reports' AS [Description]
UNION ALL
SELECT 
    15 AS ParentModuleId,
    'Monitoring' AS Name,
    'Monitoring' AS [Description];