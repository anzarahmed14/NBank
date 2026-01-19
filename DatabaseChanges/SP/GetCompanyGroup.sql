CREATE OR ALTER PROC [dbo].[GetCompanyGroup]
(
    @CompanyGroupID BIGINT = NULL,
    @CompanyGroupName NVARCHAR(100) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF (@CompanyGroupName IS NOT NULL AND LEN(@CompanyGroupName) > 0)
    BEGIN
        SELECT
            CG.CompanyGroupID,
            CG.CompanyGroupCode,
            CG.CompanyGroupName,
            CG.IsActive,
            CONCAT(CG.CompanyGroupName, ' ', CG.CompanyGroupCode) AS FULL_NAME,
            STRING_AGG(CM.CompanyShortName, ' | ') AS CompanyNames
        FROM CompanyGroupMaster CG
        LEFT JOIN MapCompanyGroup M
            ON M.CompanyGroupID = CG.CompanyGroupID
        LEFT JOIN CompanyMaster CM
            ON CM.CompanyID = M.CompanyID
        WHERE CONCAT(CG.CompanyGroupName, ' ', CG.CompanyGroupCode)
              LIKE '%' + @CompanyGroupName + '%'
        GROUP BY
            CG.CompanyGroupID,
            CG.CompanyGroupCode,
            CG.CompanyGroupName,
            CG.IsActive
        ORDER BY CG.CompanyGroupName;
    END
    ELSE
    BEGIN
        SELECT
            CG.CompanyGroupID,
            CG.CompanyGroupCode,
            CG.CompanyGroupName,
            CG.IsActive,
            STRING_AGG(CM.CompanyShortName, ' | ') AS CompanyNames
        FROM CompanyGroupMaster CG
        LEFT JOIN MapCompanyGroup M
            ON M.CompanyGroupID = CG.CompanyGroupID
        LEFT JOIN CompanyMaster CM
            ON CM.CompanyID = M.CompanyID
        WHERE CG.CompanyGroupID = ISNULL(@CompanyGroupID, CG.CompanyGroupID)
        GROUP BY
            CG.CompanyGroupID,
            CG.CompanyGroupCode,
            CG.CompanyGroupName,
            CG.IsActive
        ORDER BY CG.CompanyGroupName;
    END
END
GO


