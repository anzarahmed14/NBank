

CREATE TABLE [dbo].[MapCompanyGroup](
	[MapCompanyGroupId] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyGroupId] [bigint] NULL,
	[CompanyId] [bigint] NULL,
 CONSTRAINT [PK_MapCompanyGroup] PRIMARY KEY CLUSTERED 
(
	[MapCompanyGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


