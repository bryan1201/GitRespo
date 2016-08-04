/****** Object:  Table [dbo].[BuyInfoes]    Script Date: 3/22/2016 7:26:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BuyInfoes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DeviceId] [nvarchar](50) NOT NULL,
	[ProductId] [nvarchar](50) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Amount] [float] NOT NULL,
	[BuyTime] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.BuyInfoes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[BuyInfoes]  WITH NOCHECK ADD  CONSTRAINT [FK_BuyInfoes_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO

ALTER TABLE [dbo].[BuyInfoes] CHECK CONSTRAINT [FK_BuyInfoes_Products]
GO


