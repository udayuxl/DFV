
/****** Object:  Table [Emerge].[tblSM_Level1_Budget]    Script Date: 07/30/2014 16:39:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Emerge].[tblSM_Level1_Budget](
	[pk_Sm_Level1_Budget_id] [int] IDENTITY(1,1) NOT NULL,
	[fk_Sm_Level1_id] [int] NOT NULL,
	[fk_Program_id] [int] NOT NULL,
	[budget] [numeric](16, 2) NOT NULL,
	[budget_Allocated] [numeric](16, 2) NOT NULL,
 CONSTRAINT [tblSM_Level1_Budget_PK] PRIMARY KEY CLUSTERED 
(
	[pk_Sm_Level1_Budget_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Emerge].[tblSM_Level1_Budget]  WITH CHECK ADD  CONSTRAINT [tblProgram_tblSM_Level1_Budget_FK1] FOREIGN KEY([fk_Program_id])
REFERENCES [Emerge].[tblProgram] ([pk_Program_Id])
GO

ALTER TABLE [Emerge].[tblSM_Level1_Budget] CHECK CONSTRAINT [tblProgram_tblSM_Level1_Budget_FK1]
GO

ALTER TABLE [Emerge].[tblSM_Level1_Budget]  WITH CHECK ADD  CONSTRAINT [tblSM_Level1_tblSM_Level1_Budget_FK1] FOREIGN KEY([fk_Sm_Level1_id])
REFERENCES [Emerge].[tblSM_Level1] ([pk_Sm_Level1_id])
GO

ALTER TABLE [Emerge].[tblSM_Level1_Budget] CHECK CONSTRAINT [tblSM_Level1_tblSM_Level1_Budget_FK1]
GO

ALTER TABLE [Emerge].[tblSM_Level1_Budget] ADD  DEFAULT ((0)) FOR [budget]
GO

ALTER TABLE [Emerge].[tblSM_Level1_Budget] ADD  DEFAULT ((0)) FOR [budget_Allocated]
GO


