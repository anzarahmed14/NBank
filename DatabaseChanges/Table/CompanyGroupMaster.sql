

CREATE TABLE [dbo].[CompanyGroupMaster](
	[CompanyGroupID] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyGroupCode] [nvarchar](20) NULL,
	[CompanyGroupName] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_CompanyGroupMaster] PRIMARY KEY CLUSTERED 
(
	[CompanyGroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


