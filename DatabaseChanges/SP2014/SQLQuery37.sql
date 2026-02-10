ALTER PROC [dbo].[GetChequeReport]
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

    /* =========================================================
       NORMALIZE STRING PARAMETERS (Crystal + Grid Safe)
    ========================================================= */
    SET @ChequeNo       = NULLIF(LTRIM(RTRIM(@ChequeNo)), '');
    SET @AccountSubName = NULLIF(LTRIM(RTRIM(@AccountSubName)), '');
    SET @ERPID          = NULLIF(LTRIM(RTRIM(@ERPID)), '');

    /* =========================================================
       RESOLVE SINGLE DATE COLUMN
    ========================================================= */
    DECLARE @FilterDateColumn NVARCHAR(128);

    SET @FilterDateColumn =
        CASE @DateType
            WHEN 'CED' THEN 'ChequeEntryDate'
            WHEN 'CID' THEN 'ChequeIssueDate'
            WHEN 'CCD' THEN 'ChequeClearDate'
            ELSE NULL
        END;

    DECLARE @SQLQuery NVARCHAR(MAX);
    DECLARE @ParamDefinition NVARCHAR(MAX);

    /* =========================================================
       BASE QUERY
    ========================================================= */
    SET @SQLQuery = N'
        SELECT *,
               (ISNULL(ChequeAmount,0) + ISNULL(ChequeAmountTDS,0)) AS NetAmount
        FROM View_ChequeEntry V
        WHERE 1 = 1
    ';

    /* =========================================================
       USER SECURITY
    ========================================================= */
    SET @SQLQuery += N'
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

    /* =========================================================
       DATE FILTER (ONLY ONE DATE COLUMN)
    ========================================================= */
    IF @FilterDateColumn IS NOT NULL
    BEGIN
        SET @SQLQuery += N'
            AND V.' + QUOTENAME(@FilterDateColumn) + N' >= COALESCE(@StartDate, V.' + QUOTENAME(@FilterDateColumn) + N')
            AND V.' + QUOTENAME(@FilterDateColumn) + N' <= COALESCE(@EndDate,   V.' + QUOTENAME(@FilterDateColumn) + N')';
    END;

    /* =========================================================
       OPTIONAL FILTERS
    ========================================================= */
    IF @ChequeNo IS NOT NULL
        SET @SQLQuery += N' AND V.ChequeNo = @ChequeNo';

    IF @AccountID IS NOT NULL
        SET @SQLQuery += N' AND V.AccountID = @AccountID';

    IF @BankID IS NOT NULL
        SET @SQLQuery += N' AND V.BankID = @BankID';

    IF @ChequeStatusID IS NOT NULL
        SET @SQLQuery += N' AND V.ChequeStatusID = @ChequeStatusID';

    IF @ChequeEntryID IS NOT NULL
        SET @SQLQuery += N' AND V.ChequeEntryID = @ChequeEntryID';

    IF @CompanyID IS NOT NULL
        SET @SQLQuery += N' AND V.CompanyID = @CompanyID';

    IF @TypeID IS NOT NULL
        SET @SQLQuery += N' AND V.TypeID = @TypeID';

    IF @SubTypeID IS NOT NULL
        SET @SQLQuery += N' AND V.SubTypeID = @SubTypeID';

    IF @ChequeTypeID IS NOT NULL
        SET @SQLQuery += N' AND V.ChequeTypeID = @ChequeTypeID';

    IF @ParameterID IS NOT NULL
        SET @SQLQuery += N' AND V.ParameterID = @ParameterID';

    IF @ProjectID IS NOT NULL
        SET @SQLQuery += N' AND V.ProjectID = @ProjectID';

    IF @AccountSubName IS NOT NULL
        SET @SQLQuery += N' AND V.AccountSubName LIKE ''%'' + @AccountSubName + ''%''';

    IF @ERPID IS NOT NULL
        SET @SQLQuery += N' AND V.ERPID LIKE ''%'' + @ERPID + ''%''';

    /* =========================================================
       ORDER BY (SAME DATE COLUMN)
    ========================================================= */
    IF @FilterDateColumn IS NOT NULL
    BEGIN
        SET @SQLQuery += N'
            ORDER BY V.' + QUOTENAME(@FilterDateColumn) + N' DESC,
                     V.ChequeEntryID DESC';
    END;

    /* =========================================================
       PARAM DEFINITIONS
    ========================================================= */
    SET @ParamDefinition = N'
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

    /* =========================================================
       EXECUTE
    ========================================================= */
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
