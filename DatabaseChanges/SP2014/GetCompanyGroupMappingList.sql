IF OBJECT_ID('dbo.GetCompanyGroupMappingList', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetCompanyGroupMappingList;
GO

CREATE PROC [dbo].[GetCompanyGroupMappingList]
(
    @CompanyGroupName NVARCHAR(100) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CG.CompanyGroupID,
        CG.CompanyGroupName,

        /* Pipe-separated company names */
        STUFF((
            SELECT ' | ' + CM2.CompanyName
            FROM dbo.MapCompanyGroup M2
            INNER JOIN dbo.CompanyMaster CM2
                ON CM2.CompanyID = M2.CompanyId
            WHERE M2.CompanyGroupId = CG.CompanyGroupID
            FOR XML PATH(''), TYPE
        ).value('.', 'NVARCHAR(MAX)'), 1, 3, '') AS CompanyNames,

        COUNT(DISTINCT M.CompanyId) AS CompanyCount
    FROM dbo.MapCompanyGroup M
    INNER JOIN dbo.CompanyGroupMaster CG
        ON CG.CompanyGroupID = M.CompanyGroupId
    INNER JOIN dbo.CompanyMaster CM
        ON CM.CompanyID = M.CompanyId
    WHERE
        (
            @CompanyGroupName IS NULL
            OR @CompanyGroupName = ''
            OR CG.CompanyGroupName LIKE '%' + @CompanyGroupName + '%'
        )
    GROUP BY
        CG.CompanyGroupID,
        CG.CompanyGroupName
    ORDER BY
        CG.CompanyGroupName;
END;
GO
