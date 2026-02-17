/****** Object:  UserDefinedTableType [dbo].[ChequeEntryImportType]    Script Date: 16-02-2026 11:01:16 PM ******/
DROP TYPE [dbo].[ChequeEntryImportType]
GO

/****** Object:  UserDefinedTableType [dbo].[ChequeEntryImportType]    Script Date: 16-02-2026 11:01:16 PM ******/
CREATE TYPE [dbo].[ChequeEntryImportType] AS TABLE(
	[EntryDate] [datetime] NULL,
	[IssueDate] [datetime] NULL,
	[ProjectID] [bigint] NULL,
	[AccountID] [bigint] NULL,
	[AccountSubName] [nvarchar](100) NULL,
	[BankID] [bigint] NULL,
	[ChequeNo] [nvarchar](20) NULL,
	[TypeID] [bigint] NULL,
	[SubTypeID] [bigint] NULL,
	[ParameterID] [bigint] NULL,
	[ChequeAmount] [decimal](18, 2) NULL,
	[CompanyID] [bigint] NULL,
	[Narration] [nvarchar](MAX) NULL
)
GO


