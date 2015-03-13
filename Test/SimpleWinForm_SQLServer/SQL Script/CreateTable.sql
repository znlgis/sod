CREATE TABLE [dbo].[User] (
    [UserID]       INT             NOT NULL,
    [UserName]     NVARCHAR (50)   NULL,
    [UserType]     INT             NULL,
    [RegisterDate] DATETIME        NULL,
    [Expenditure]  DECIMAL (18, 4) NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC)
);
INSERT INTO [dbo].[User] ([UserID], [UserName], [UserType], [RegisterDate], [Expenditure]) VALUES (1, N'Admin', NULL, N'1990-01-01 00:00:00', NULL)
