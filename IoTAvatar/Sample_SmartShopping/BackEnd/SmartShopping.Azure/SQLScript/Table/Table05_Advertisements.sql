/****** Object:  Table [dbo].[Advertisements]    Script Date: 3/31/2016 12:59:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Advertisements](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BeaconId] [nvarchar](50) NOT NULL,
	[TargetDeviceId] [nvarchar](50) NOT NULL,
	[SignalStrength] [int] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Advertisements] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[Advertisements]  WITH CHECK ADD  CONSTRAINT [FK_Advertisements_Beacons] FOREIGN KEY([BeaconId])
REFERENCES [dbo].[Beacons] ([BeaconId])
GO

ALTER TABLE [dbo].[Advertisements] CHECK CONSTRAINT [FK_Advertisements_Beacons]
GO


