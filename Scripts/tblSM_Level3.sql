
/****** Object:  Table [Emerge].[tblSM_Level3]    Script Date: 07/30/2014 16:43:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [Emerge].[tblSM_Level3](
	[pk_Sm_Level3_id] [int] IDENTITY(1,1) NOT NULL,
	[designation_name] [varchar](100) NOT NULL,
	[level3_sales_manager_id] [uniqueidentifier] NULL,
	[budget] [numeric](16, 2) NOT NULL,
	[fk_Sm_Level2_id] [int] NOT NULL,
	[Active] [bit] NULL,
 CONSTRAINT [tblSM_Level3_PK] PRIMARY KEY CLUSTERED 
(
	[pk_Sm_Level3_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [Emerge].[tblSM_Level3]  WITH CHECK ADD  CONSTRAINT [aspnet_Users_tblSM_Level3_FK1] FOREIGN KEY([level3_sales_manager_id])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO

ALTER TABLE [Emerge].[tblSM_Level3] CHECK CONSTRAINT [aspnet_Users_tblSM_Level3_FK1]
GO

ALTER TABLE [Emerge].[tblSM_Level3]  WITH CHECK ADD  CONSTRAINT [tblSM_Level2_tblSM_Level3_FK1] FOREIGN KEY([fk_Sm_Level2_id])
REFERENCES [Emerge].[tblSM_Level2] ([pk_Sm_Level2_id])
GO

ALTER TABLE [Emerge].[tblSM_Level3] CHECK CONSTRAINT [tblSM_Level2_tblSM_Level3_FK1]
GO

ALTER TABLE [Emerge].[tblSM_Level3] ADD  DEFAULT ((0)) FOR [budget]
GO

ALTER TABLE [Emerge].[tblSM_Level3] ADD  DEFAULT ((1)) FOR [Active]
GO


