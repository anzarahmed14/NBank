CREATE TABLE ImportLog
(
    ImportLogID     BIGINT IDENTITY(1,1) PRIMARY KEY,

    CompanyID       BIGINT,
    BankID          BIGINT,

    TotalRows        INT,

    CreatedUserID   BIGINT,
    CreatedDate     DATETIME DEFAULT GETDATE(),

    FileName        NVARCHAR(200) NULL,
)
GO
