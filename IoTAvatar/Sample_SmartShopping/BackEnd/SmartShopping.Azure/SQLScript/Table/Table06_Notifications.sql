/****** Object:  Table [dbo].[Notifications]    Script Date: 3/31/2016 12:58:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Notifications](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AdvertisementId] [bigint] NOT NULL,
	[SendTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Advertisements] FOREIGN KEY([AdvertisementId])
REFERENCES [dbo].[Advertisements] ([Id])
GO

ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Advertisements]
GO


