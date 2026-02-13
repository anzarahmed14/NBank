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

    /* ===============================
       NORMALIZE STRING PARAMETERS
    =============================== */
    SET @ChequeNo       = NULLIF(LTRIM(RTRIM(@ChequeNo)), '');
    SET @AccountSubName = NULLIF(LTRIM(RTRIM(@AccountSubName)), '');
    SET @ERPID          = NULLIF(LTRIM(RTRIM(@ERPID)), '');

    /* ===============================
       DEFAULT DATE RANGE
    =============================== */
    SET @StartDate = ISNULL(@StartDate, '19000101');
    SET @EndDate   = ISNULL(@EndDate,   '99991231');

    /* ===============================
       MAIN QUERY
    =============================== */
    SELECT
        V.*,
        (ISNULL(V.ChequeAmount,0) + ISNULL(V.ChequeAmountTDS,0)) AS NetAmount
    FROM View_ChequeEntry V
    WHERE
        /* USER SECURITY */
        (
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

        /* DATE FILTER (ONLY ONE ACTIVE) */
        AND (
                (@DateType = 'CED' AND V.ChequeEntryDate BETWEEN @StartDate AND @EndDate)
             OR (@DateType = 'CID' AND V.ChequeIssueDate BETWEEN @StartDate AND @EndDate)
             OR (@DateType = 'CCD' AND V.ChequeClearDate BETWEEN @StartDate AND @EndDate)
             OR (@DateType IS NULL)
            )

        /* OPTIONAL FILTERS */
        AND (@ChequeNo       IS NULL OR V.ChequeNo       = @ChequeNo)
        AND (@AccountID      IS NULL OR V.AccountID      = @AccountID)
        AND (@BankID         IS NULL OR V.BankID         = @BankID)
        AND (@ChequeStatusID IS NULL OR V.ChequeStatusID = @ChequeStatusID)
        AND (@ChequeEntryID  IS NULL OR V.ChequeEntryID  = @ChequeEntryID)
        AND (@CompanyID      IS NULL OR V.CompanyID      = @CompanyID)
        AND (@TypeID         IS NULL OR V.TypeID         = @TypeID)
        AND (@SubTypeID      IS NULL OR V.SubTypeID      = @SubTypeID)
        AND (@ChequeTypeID   IS NULL OR V.ChequeTypeID   = @ChequeTypeID)
        AND (@ParameterID    IS NULL OR V.ParameterID    = @ParameterID)
        AND (@ProjectID      IS NULL OR V.ProjectID      = @ProjectID)
        AND (@AccountSubName IS NULL OR V.AccountSubName LIKE '%' + @AccountSubName + '%')
        AND (@ERPID          IS NULL OR V.ERPID          LIKE '%' + @ERPID + '%')

    ORDER BY
        CASE @DateType
            WHEN 'CED' THEN V.ChequeEntryDate
            WHEN 'CID' THEN V.ChequeIssueDate
            WHEN 'CCD' THEN V.ChequeClearDate
			ELSE V.ChequeEntryDate 
        END DESC,
        V.ChequeEntryID DESC;
END;
GO
