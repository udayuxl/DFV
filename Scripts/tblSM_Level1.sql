
/****** Object:  Table [Emerge].[tblSM_Level1]    Script Date: 07/30/2014 16:37:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [Emerge].[tblSM_Level1](
	[pk_Sm_Level1_id] [int] IDENTITY(1,1) NOT NULL,
	[designation_name] [varchar](100) NOT NULL,
	[level1_sales_manager_id] [uniqueidentifier] NULL,
	[budget] [numeric](16, 2) NOT NULL,
	[Active] [bit] NULL,
 CONSTRAINT [tblSM_Level1_PK] PRIMARY KEY CLUSTERED 
(
	[pk_Sm_Level1_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [Emerge].[tblSM_Level1]  WITH CHECK ADD  CONSTRAINT [aspnet_Users_tblSM_Level1_FK1] FOREIGN KEY([level1_sales_manager_id])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO

ALTER TABLE [Emerge].[tblSM_Level1] CHECK CONSTRAINT [aspnet_Users_tblSM_Level1_FK1]
GO

ALTER TABLE [Emerge].[tblSM_Level1] ADD  DEFAULT ((0)) FOR [budget]
GO

ALTER TABLE [Emerge].[tblSM_Level1] ADD  DEFAULT ((1)) FOR [Active]
GO


