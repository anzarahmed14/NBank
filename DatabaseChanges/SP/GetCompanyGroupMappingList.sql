CREATE OR ALTER   PROC [dbo].[GetCompanyGroupMappingList]
(
    @CompanyGroupName NVARCHAR(100) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CG.CompanyGroupID,
        CG.CompanyGroupName,
        COUNT(M.CompanyId) AS CompanyCount
    FROM dbo.MapCompanyGroup M
    INNER JOIN dbo.CompanyGroupMaster CG
        ON CG.CompanyGroupID = M.CompanyGroupId
    WHERE
        (@CompanyGroupName IS NULL OR @CompanyGroupName = ''
         OR CG.CompanyGroupName LIKE '%' + @CompanyGroupName + '%')
    GROUP BY
        CG.CompanyGroupID,
        CG.CompanyGroupName
    ORDER BY
        CG.CompanyGroupName;
END
GO


