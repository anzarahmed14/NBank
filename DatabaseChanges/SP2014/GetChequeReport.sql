IF OBJECT_ID('dbo.GetChequeReport', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetChequeReport;
GO

CREATE PROC [dbo].[GetChequeReport]
(
    @DateType         NVARCHAR(20) = NULL,
    @StartDate        DATE = NULL,
    @EndDate          DATE = NULL,
    @ChequeStatusID   BIGINT = NULL,
    @BankID           BIGINT = NULL,
    @ChequeNo         NVARCHAR(20) = NULL,
    @AccountID        BIGINT = NULL,
    @ChequeEntryID    BIGINT = NULL,
    @CompanyID        BIGINT = NULL,
    @ParameterID      BIGINT = NULL,
    @ProjectID        BIGINT = NULL,
    @ChequeTypeID     BIGINT = NULL,
    @SubTypeID        BIGINT = NULL,
    @TypeID           BIGINT = NULL,
    @AccountSubName   NVARCHAR(100) = NULL,
    @ERPID            NVARCHAR(MAX) = NULL,
    @UserId           BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQLQuery NVARCHAR(MAX);
    DECLARE @ParamDefinition NVARCHAR(MAX);

    SET @SQLQuery = '
        SELECT *,
               (ISNULL(ChequeAmount,0) + ISNULL(ChequeAmountTDS,0)) AS NetAmount
        FROM View_ChequeEntry V
        WHERE 1 = 1
    ';

    /* USER SECURITY (ALWAYS APPLIED) */
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

    /* DATE FILTERS */
    IF (@DateType = 'CED')
        SET @SQLQuery += '
            AND V.ChequeEntryDate BETWEEN
                COALESCE(@StartDate, V.ChequeEntryDate)
            AND COALESCE(@EndDate, V.ChequeEntryDate)';

    IF (@DateType = 'CID')
        SET @SQLQuery += '
            AND V.ChequeIssueDate BETWEEN
                COALESCE(@StartDate, V.ChequeIssueDate)
            AND COALESCE(@EndDate, V.ChequeIssueDate)';

    IF (@DateType = 'CCD')
        SET @SQLQuery += '
            AND V.ChequeClearDate BETWEEN
                COALESCE(@StartDate, V.ChequeClearDate)
            AND COALESCE(@EndDate, V.ChequeClearDate)';

    /* OTHER FILTERS */
    IF @ChequeNo IS NOT NULL
        SET @SQLQuery += ' AND V.ChequeNo = @ChequeNo';

    IF @AccountID IS NOT NULL
        SET @SQLQuery += ' AND V.AccountID = @AccountID';

    IF @BankID IS NOT NULL
        SET @SQLQuery += ' AND V.BankID = @BankID';

    IF @ChequeStatusID IS NOT NULL
        SET @SQLQuery += ' AND V.ChequeStatusID = @ChequeStatusID';

    IF @ChequeEntryID IS NOT NULL
        SET @SQLQuery += ' AND V.ChequeEntryID = @ChequeEntryID';

    IF @CompanyID IS NOT NULL
        SET @SQLQuery += ' AND V.CompanyID = @CompanyID';

    IF @TypeID IS NOT NULL
        SET @SQLQuery += ' AND V.TypeID = @TypeID';

    IF @SubTypeID IS NOT NULL
        SET @SQLQuery += ' AND V.SubTypeID = @SubTypeID';

    IF @ChequeTypeID IS NOT NULL
        SET @SQLQuery += ' AND V.ChequeTypeID = @ChequeTypeID';

    IF @ParameterID IS NOT NULL
        SET @SQLQuery += ' AND V.ParameterID = @ParameterID';

    IF @ProjectID IS NOT NULL
        SET @SQLQuery += ' AND V.ProjectID = @ProjectID';

    IF @AccountSubName IS NOT NULL
        SET @SQLQuery += ' AND V.AccountSubName LIKE ''%'' + @AccountSubName + ''%''';

    IF @ERPID IS NOT NULL
        SET @SQLQuery += ' AND V.ERPID LIKE ''%'' + @ERPID + ''%''';

    /* ORDER BY */
    IF (@DateType = 'CED')
        SET @SQLQuery += ' ORDER BY V.ChequeEntryDate DESC, V.ChequeEntryID DESC';

    IF (@DateType = 'CID')
        SET @SQLQuery += ' ORDER BY V.ChequeIssueDate DESC, V.ChequeEntryID DESC';

    IF (@DateType = 'CCD')
        SET @SQLQuery += ' ORDER BY V.ChequeClearDate DESC, V.ChequeEntryID DESC';

    SET @ParamDefinition = '
        @DateType NVARCHAR(20),
        @StartDate DATE,
        @EndDate DATE,
        @ChequeStatusID BIGINT,
        @BankID BIGINT,
        @AccountID BIGINT,
        @ChequeEntryID BIGINT,
        @ChequeNo NVARCHAR(20),
        @CompanyID BIGINT,
        @ParameterID BIGINT,
        @ProjectID BIGINT,
        @ChequeTypeID BIGINT,
        @SubTypeID BIGINT,
        @TypeID BIGINT,
        @AccountSubName NVARCHAR(100),
        @ERPID NVARCHAR(MAX),
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
        @ChequeEntryID,
        @ChequeNo,
        @CompanyID,
        @ParameterID,
        @ProjectID,
        @ChequeTypeID,
        @SubTypeID,
        @TypeID,
        @AccountSubName,
        @ERPID,
        @UserId;
END;
GO
