DROP TYPE dbo.ChequeEntryImportType
GO

CREATE TYPE dbo.ChequeEntryImportType AS TABLE
(
    ChequeEntryDate   DATETIME,
    ProjectID         BIGINT,
    AccountID         BIGINT,
    AccountSubName    NVARCHAR(100),
    BankID            BIGINT,
    ChequeNo          NVARCHAR(20),
    TypeID            BIGINT,
    SubTypeID         BIGINT,
    ParameterID       BIGINT,
    ChequeAmount      DECIMAL(18,2),
    CompanyID         BIGINT
)
GO
