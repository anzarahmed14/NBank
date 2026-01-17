ALTER PROC [dbo].[GetCompany]
(
    @CompanyID   BIGINT = NULL,
    @CompanyName NVARCHAR(100) = NULL,
    @UserId      BIGINT = NULL   -- NEW PARAMETER
)
AS
BEGIN
    SET NOCOUNT ON;

    /* ================================
       CASE 1: UserId passed (> 0)
       Return companies mapped to
       user's company groups
       ================================ */
    IF (@UserId IS NOT NULL AND @UserId > 0)
    BEGIN
        SELECT DISTINCT
            CM.*
        FROM CompanyMaster CM
        INNER JOIN MapCompanyGroup MCG
            ON MCG.CompanyID = CM.CompanyID
        INNER JOIN MapUserCompanyGroup MUCG
            ON MUCG.CompanyGroupID = MCG.CompanyGroupID
           AND MUCG.UserID = @UserId
        WHERE
            (@CompanyID IS NULL OR CM.CompanyID = @CompanyID)
        AND
            (@CompanyName IS NULL OR @CompanyName = ''
             OR CM.CompanyName LIKE '%' + @CompanyName + '%')
        ORDER BY CM.CompanyName;

        RETURN;
    END

    /* ================================
       CASE 2: Normal behavior
       (UserId not passed or = 0)
       ================================ */
    IF (@CompanyName IS NOT NULL AND LEN(@CompanyName) > 0)
    BEGIN
        SELECT *
        FROM CompanyMaster
        WHERE CompanyName LIKE '%' + @CompanyName + '%'
        ORDER BY CompanyName;
    END
    ELSE
    BEGIN
        SELECT *
        FROM CompanyMaster
        WHERE CompanyID = ISNULL(@CompanyID, CompanyID)
        ORDER BY CompanyName;
    END
END
GO


--EXEC GetCompany null, null ,40005

