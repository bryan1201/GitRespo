/****** Object:  Table [dbo].[Beacons]    Script Date: 3/23/2016 4:08:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Beacons](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BeaconId] [nvarchar](50) NOT NULL,
	[ProductId] [nvarchar](50) NOT NULL,
	[StoreId] [nvarchar](50) NOT NULL,
	[InFilter] [int] NULL,
	[OutFilter] [int] NULL,
	[Xaxis] [int] NULL,
	[Yaxis] [int] NULL,
	[Longitude] [float] NULL,
	[LatuLatitude] [float] NULL,
 CONSTRAINT [PK_Beacons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[BeaconId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
UNIQUE NONCLUSTERED 
(
	[BeaconId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[Beacons]  WITH CHECK ADD  CONSTRAINT [FK_Beacons_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO

ALTER TABLE [dbo].[Beacons] CHECK CONSTRAINT [FK_Beacons_Products]
GO

ALTER TABLE [dbo].[Beacons]  WITH CHECK ADD  CONSTRAINT [FK_Beacons_Stores] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Stores] ([StoreId])
GO

ALTER TABLE [dbo].[Beacons] CHECK CONSTRAINT [FK_Beacons_Stores]
GO


