CREATE TABLE [dbo].[PPMPProjects] (
    [PPMPProjectId]   INT             IDENTITY (1, 1) NOT NULL,
    [PPMPId]          INT             NOT NULL,
    [ProjectName]     NVARCHAR (MAX)  NULL,
    [Description]     NVARCHAR (MAX)  NULL,
    [Quarter]         INT             NOT NULL,
    [Cost]            DECIMAL (19, 5) NOT NULL,
    [ProjectStatus]   NVARCHAR (20)   NULL,
    [IsActive]        BIT             NOT NULL,
    [CreatedByUserId] INT             NOT NULL,
    [CreatedDate]     DATETIME        NOT NULL,
    [UpdatedByUserId] INT             NULL,
    [UpdatedDate]     DATETIME        NULL,
    [DeletedByUserId] INT             NULL,
    [DeletedDate]     DATETIME        NULL,
    [AccountCodeId]   INT             NULL,
    CONSTRAINT [PK_PPMPProjects] PRIMARY KEY CLUSTERED ([PPMPProjectId] ASC),
    CONSTRAINT [FK_PPMPProjects_PPMPProjects] FOREIGN KEY ([AccountCodeId]) REFERENCES [dbo].[SS_AccountCode] ([AccountCodeId]),
    CONSTRAINT [FK_PPMPProjects_PPMPs] FOREIGN KEY ([PPMPId]) REFERENCES [dbo].[PPMPs] ([PPMPId])
);

