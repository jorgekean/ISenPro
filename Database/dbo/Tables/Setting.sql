CREATE TABLE [dbo].[Setting] (
    [Type]        VARCHAR (50)  NOT NULL,
    [Code]        VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (200) NULL,
    [AltDesc]     VARCHAR (200) NULL,
    [Enabled]     BIT           NOT NULL,
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([Type] ASC, [Code] ASC)
);

