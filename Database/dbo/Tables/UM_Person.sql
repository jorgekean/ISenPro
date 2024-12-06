CREATE TABLE [dbo].[UM_Person] (
    [PersonId]        INT            IDENTITY (1, 1) NOT NULL,
    [LastName]        NVARCHAR (100) NULL,
    [FirstName]       NVARCHAR (100) NULL,
    [MiddleName]      NVARCHAR (100) NULL,
    [Address]         NVARCHAR (200) NULL,
    [IsHeadOfOffice]  BIT            NOT NULL,
    [Email]           NVARCHAR (100) NULL,
    [ContactNo]       NVARCHAR (100) NULL,
    [Thumbnail]       NVARCHAR (MAX) NULL,
    [Remarks]         NVARCHAR (MAX) NULL,
    [Designation]     NVARCHAR (200) NULL,
    [EmployeeTitle]   INT            NULL,
    [EmployeeStatus]  INT            NULL,
    [IsActive]        BIT            NOT NULL,
    [CreatedByUserId] BIGINT         NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [DepartmentId]    INT            NULL,
    [SectionId]       INT            NULL,
    CONSTRAINT [PK_UM_Person] PRIMARY KEY CLUSTERED ([PersonId] ASC),
    CONSTRAINT [FK_UM_Person_UM_Department] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[UM_Department] ([DepartmentId]),
    CONSTRAINT [FK_UM_Person_UM_Section] FOREIGN KEY ([SectionId]) REFERENCES [dbo].[UM_Section] ([SectionId])
);



