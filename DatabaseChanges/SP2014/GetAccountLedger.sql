IF OBJECT_ID('dbo.GetAccountLedger', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetAccountLedger;
GO

CREATE PROC [dbo].[GetAccountLedger]
(
    @DateType NVARCHAR(20) = NULL,
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL,
    @ChequeStatusID BIGINT = NULL,
    @BankID BIGINT = NULL,
    @ChequeNo NVARCHAR(20) = NULL,
    @AccountID BIGINT = NULL,
    @ProjectID BIGINT = NULL,
    @UserId BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQLQuery NVARCHAR(MAX);
    DECLARE @ParamDefinition NVARCHAR(MAX);

    SET @SQLQuery = '
        SELECT *,
               CASE ChequeTypeID
                    WHEN 1 THEN ISNULL(ChequeAmount,0) + ISNULL(ChequeAmountTDS,0)
                    ELSE 0
               END AS Debit,
               CASE ChequeTypeID
                    WHEN 2 THEN ISNULL(ChequeAmount,0) + ISNULL(ChequeAmountTDS,0)
                    ELSE 0
               END AS Credit
        FROM View_ChequeEntry V
        WHERE 1 = 1
    ';

    /* DATE FILTER (ALWAYS APPLIED) */
    SET @SQLQuery += '
        AND V.ChequeEntryDate BETWEEN
            COALESCE(@StartDate, V.ChequeEntryDate)
        AND COALESCE(@EndDate, V.ChequeEntryDate)
    ';

    /* USER SECURITY FILTER (NON-BYPASSABLE) */
    SET @SQLQuery += '
        AND (
            @UserId = 0
            OR EXISTS
            (
                SELECT 1
                FROM MapCompanyGroup MCG
                INNER JOIN MapUserCompanyGroup MUCG
                    ON MUCG.CompanyGroupID = MCG.CompanyGroupID
                WHERE MUCG.UserID = @UserId
                  AND MCG.CompanyID = V.CompanyID
            )
        )
    ';

    /* OPTIONAL FILTERS */
    IF (@AccountID > 0)
        SET @SQLQuery += ' AND V.AccountID = @AccountID';

    IF (@ProjectID > 0)
        SET @SQLQuery += ' AND V.ProjectID = @ProjectID';

    /* ORDER BY */
    IF (@DateType = 'CED')
        SET @SQLQuery += ' ORDER BY V.ChequeEntryDate DESC';

    IF (@DateType = 'CID')
        SET @SQLQuery += ' ORDER BY V.ChequeIssueDate DESC';

    IF (@DateType = 'CCD')
        SET @SQLQuery += ' ORDER BY V.ChequeClearDate DESC';

    SET @ParamDefinition = '
        @DateType NVARCHAR(20),
        @StartDate DATETIME,
        @EndDate DATETIME,
        @ChequeStatusID BIGINT,
        @BankID BIGINT,
        @AccountID BIGINT,
        @ProjectID BIGINT,
        @ChequeNo NVARCHAR(20),
        @UserId BIGINT';

    EXEC sp_executesql
        @SQLQuery,
        @ParamDefinition,
        @DateType,
        @StartDate,
        @EndDate,
        @ChequeStatusID,
        @BankID,
        @AccountID,
        @ProjectID,
        @ChequeNo,
        @UserId;
END;
GO
