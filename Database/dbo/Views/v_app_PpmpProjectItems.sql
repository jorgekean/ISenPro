
CREATE VIEW dbo.v_app_PpmpProjectItems
AS
SELECT A.BudgetYear,
       d.Name RequestingOffice,      
       pc.*
FROM dbo.PPMPs A
    JOIN dbo.PPMPProjects pc
        ON pc.PPMPId = A.PPMPId
    JOIN dbo.UM_Department d
        ON d.DepartmentId = A.RequestingOfficeId       
WHERE A.IsActive = 1
      AND pc.IsActive = 1;