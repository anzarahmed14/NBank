CREATE OR ALTER PROC [dbo].[GetUserCompanyGroupMappingList]
(
    @UserName NVARCHAR(100) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.UserId,
        U.UserName,

        /* Pipe-separated Company Group Names */
        STRING_AGG(CG.CompanyGroupName, ' | ') AS CompanyGroupNames,

        COUNT(DISTINCT M.CompanyGroupId) AS CompanyGroupCount
    FROM dbo.MapUserCompanyGroup M
    INNER JOIN dbo.UserMaster U
        ON U.UserId = M.UserId
    INNER JOIN dbo.CompanyGroupMaster CG
        ON CG.CompanyGroupId = M.CompanyGroupId
    WHERE
        (
            @UserName IS NULL
            OR @UserName = ''
            OR U.UserName LIKE '%' + @UserName + '%'
        )
    GROUP BY
        U.UserId,
        U.UserName
    ORDER BY
        U.UserName;
END
GO


