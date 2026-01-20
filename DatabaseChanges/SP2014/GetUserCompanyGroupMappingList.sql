IF OBJECT_ID('dbo.GetUserCompanyGroupMappingList', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetUserCompanyGroupMappingList;
GO

CREATE PROC [dbo].[GetUserCompanyGroupMappingList]
(
    @UserName NVARCHAR(100) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.UserId,
        U.UserName,

        /* Pipe-separated Company Group Names (SQL 2014 compatible) */
        STUFF((
            SELECT ' | ' + CG2.CompanyGroupName
            FROM dbo.MapUserCompanyGroup M2
            INNER JOIN dbo.CompanyGroupMaster CG2
                ON CG2.CompanyGroupId = M2.CompanyGroupId
            WHERE M2.UserId = U.UserId
            FOR XML PATH(''), TYPE
        ).value('.', 'NVARCHAR(MAX)'), 1, 3, '') AS CompanyGroupNames,

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
END;
GO
