/****** Object:  Table [dbo].[Devices]    Script Date: 3/22/2016 7:27:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Devices](
	[DevId] [bigint] IDENTITY(1,1) NOT NULL,
	[DeviceId] [nvarchar](50) NOT NULL,
	[DeviceKey] [nvarchar](max) NULL,
	[ConnectionString] [nvarchar](max) NULL,
	[IsUsed] [bit] NULL,
	[IsActive] [bit] NULL,
	[TimeStamp] [datetime] NULL,
 CONSTRAINT [PK_dbo.Devices] PRIMARY KEY CLUSTERED 
(
	[DevId] ASC,
	[DeviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
UNIQUE NONCLUSTERED 
(
	[DeviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[Devices] ADD  CONSTRAINT [DF_Devices_IsUsed]  DEFAULT ((0)) FOR [IsUsed]
GO

ALTER TABLE [dbo].[Devices] ADD  CONSTRAINT [DF_Devices_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO

