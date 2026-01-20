IF OBJECT_ID('dbo.GetChequeEntry', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetChequeEntry;
GO

CREATE PROC [dbo].[GetChequeEntry]
(
    @ColumnName     NVARCHAR(100) = NULL,
    @FromDate       DATETIME = NULL,
    @ToDate         DATETIME = NULL,
    @ChequeNo       NVARCHAR(20) = NULL,
    @CompanyID      BIGINT = NULL,
    @ProjectID      BIGINT = NULL,
    @BankID         BIGINT = NULL,
    @ChequeEntryID  BIGINT = NULL,
    @UserId         BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM View_ChequeEntry V
    WHERE
        CONVERT(VARCHAR(26), V.ChequeEntryDate, 23)
        BETWEEN
            COALESCE(CONVERT(VARCHAR(26), @FromDate, 23), V.ChequeEntryDate)
        AND
            COALESCE(CONVERT(VARCHAR(26), @ToDate, 23), V.ChequeEntryDate)

        AND (@ChequeNo IS NULL OR V.ChequeNO LIKE @ChequeNo + '%')
        AND (@CompanyID IS NULL OR V.CompanyID = @CompanyID)
        AND (@ProjectID IS NULL OR V.ProjectID = @ProjectID)
        AND (@BankID IS NULL OR V.BankID = @BankID)
        AND (@ChequeEntryID IS NULL OR V.ChequeEntryID = @ChequeEntryID)

        /* USER SECURITY FILTER (ALWAYS APPLIED) */
        AND
        (
            @UserId = 0   -- Admin
            OR EXISTS
            (
                SELECT 1
                FROM MapCompanyGroup MCG
                INNER JOIN MapUserCompanyGroup MUCG
                    ON MUCG.CompanyGroupID = MCG.CompanyGroupID
                WHERE
                    MUCG.UserID = @UserId
                    AND MCG.CompanyID = V.CompanyID
            )
        )
    ORDER BY
        V.ChequeEntryDate,
        V.ChequeEntryID DESC;
END;
GO
