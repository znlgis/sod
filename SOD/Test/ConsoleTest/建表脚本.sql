

/****** Object:  Table [dbo].[AuctionOperationLog]    Script Date: 08/31/2012 11:12:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF  EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='AuctionOperationLog')  DROP TABLE [AuctionOperationLog]
GO

CREATE TABLE [dbo].[AuctionOperationLog](
	[OptID] [int] IDENTITY(1,1) NOT NULL,
	[LogSource] [varchar](5)  NULL,
	[OperaterID] [int]  NULL,
	[Module] [varchar](10) NOT NULL,
	[Operation] [varchar](50) NULL,
	[AtDateTime] [datetime] NULL,
	[OptKeyValue] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[OptID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[AuctionOperationLog] ADD  DEFAULT (getdate()) FOR [AtDateTime]
GO


