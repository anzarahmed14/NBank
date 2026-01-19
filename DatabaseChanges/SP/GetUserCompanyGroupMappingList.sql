USE [NBank]
GO

/****** Object:  StoredProcedure [dbo].[GetUserCompanyGroupMappingList]    Script Date: 19-01-2026 11:49:19 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE OR ALTER   PROC [dbo].[GetUserCompanyGroupMappingList]
(
    @UserName NVARCHAR(100) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.UserId,
        U.UserName,
        COUNT(M.CompanyGroupId) AS CompanyGroupCount
    FROM dbo.MapUserCompanyGroup M
    INNER JOIN dbo.UserMaster U
        ON U.UserId = M.UserId
    WHERE
        (@UserName IS NULL OR @UserName = ''
         OR U.UserName LIKE '%' + @UserName + '%')
    GROUP BY
        U.UserId,
        U.UserName
    ORDER BY
        U.UserName;
END
GO


