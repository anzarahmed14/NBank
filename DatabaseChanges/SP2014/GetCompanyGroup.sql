IF OBJECT_ID('dbo.GetCompanyGroup', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetCompanyGroup;
GO

CREATE PROC [dbo].[GetCompanyGroup]
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
            STUFF((
                SELECT ' | ' + CM2.CompanyShortName
                FROM MapCompanyGroup M2
                LEFT JOIN CompanyMaster CM2
                    ON CM2.CompanyID = M2.CompanyID
                WHERE M2.CompanyGroupID = CG.CompanyGroupID
                FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)'), 1, 3, '') AS CompanyNames
        FROM CompanyGroupMaster CG
        WHERE CONCAT(CG.CompanyGroupName, ' ', CG.CompanyGroupCode)
              LIKE '%' + @CompanyGroupName + '%'
        ORDER BY CG.CompanyGroupName;
    END
    ELSE
    BEGIN
        SELECT
            CG.CompanyGroupID,
            CG.CompanyGroupCode,
            CG.CompanyGroupName,
            CG.IsActive,
            STUFF((
                SELECT ' | ' + CM2.CompanyShortName
                FROM MapCompanyGroup M2
                LEFT JOIN CompanyMaster CM2
                    ON CM2.CompanyID = M2.CompanyID
                WHERE M2.CompanyGroupID = CG.CompanyGroupID
                FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)'), 1, 3, '') AS CompanyNames
        FROM CompanyGroupMaster CG
        WHERE CG.CompanyGroupID = ISNULL(@CompanyGroupID, CG.CompanyGroupID)
        ORDER BY CG.CompanyGroupName;
    END
END;
GO
