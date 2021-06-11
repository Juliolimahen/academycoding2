USE [academycoding2]
GO

/****** Object:  Table [dbo].[HOURWORKED]    Script Date: 11/06/2021 17:39:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HOURWORKED](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CalledId] [int] NOT NULL,
	[DateInserted] [datetime] NOT NULL,
	[DateStarted] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[DateChange] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[HOURWORKED]  WITH CHECK ADD  CONSTRAINT [FK_CalledId] FOREIGN KEY([CalledId])
REFERENCES [dbo].[CALLED] ([Id])
GO

ALTER TABLE [dbo].[HOURWORKED] CHECK CONSTRAINT [FK_CalledId]
GO

