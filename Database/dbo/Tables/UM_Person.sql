CREATE TABLE [dbo].[UM_Person] (
    [PersonId]        INT            IDENTITY (1, 1) NOT NULL,
    [LastName]        NVARCHAR (100) NULL,
    [FirstName]       NVARCHAR (100) NOT NULL,
    [MiddleName]      NVARCHAR (50)  NULL,
    [Address]         NVARCHAR (200) NULL,
    [IsHeadOfOffice]  BIT            NOT NULL,
    [Email]           NVARCHAR (100) NULL,
    [ContactNo]       NVARCHAR (50)  NULL,
    [Designation]     NVARCHAR (50)  NULL,
    [EmployeeTitle]   INT            NULL,
    [EmployeeStatus]  INT            NULL,
    [IsActive]        BIT            NOT NULL,
    [CreatedByUserId] BIGINT         NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [DepartmentId]    INT            NULL,
    [SectionId]       INT            NULL,
    CONSTRAINT [PK_UM_Person] PRIMARY KEY CLUSTERED ([PersonId] ASC)
);

