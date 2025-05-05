
-- Insert FilterCriteria settings
INSERT INTO  dbo.Setting (Type, Code, Description, AltDesc, Enabled)
VALUES 
('FilterCriteria', 11, 'TransofMyDepartment', 'View only transaction of my office', 1),
('FilterCriteria', 12, 'MyTransaction', 'View only my transactions', 1),
('FilterCriteria', 13, 'TransNeedMyApproval', 'View only transactions that needs my approval', 1),
('FilterCriteria', 14, 'ApprovedTransactions', 'View only Approved Transactions', 1),
('FilterCriteria', 15, 'Supplies', 'View only Supplies', 1),
('FilterCriteria', 16, 'Equipments', 'View only Equipment', 1),
('FilterCriteria', 17, 'TransInBureau', 'View all transactions specific to the following Bureaus :', 1),
('FilterCriteria', 18, 'TransInDepartment', 'View all transactions specific to the following Offices :', 1),
('FilterCriteria', 19, 'TransInSection', 'View all transactions specific to the following Sections :', 1);