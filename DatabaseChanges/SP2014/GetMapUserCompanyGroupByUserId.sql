IF OBJECT_ID('dbo.GetMapUserCompanyGroupByUserId', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetMapUserCompanyGroupByUserId;
GO

CREATE PROC [dbo].[GetMapUserCompanyGroupByUserId]
(
    @UserId BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        M.MapUserCompanyGroupId,
        M.UserId,
        M.CompanyGroupId,
        CG.CompanyGroupName
    FROM dbo.MapUserCompanyGroup M
    INNER JOIN dbo.CompanyGroupMaster CG
        ON CG.CompanyGroupID = M.CompanyGroupId
    WHERE M.UserId = @UserId
    ORDER BY CG.CompanyGroupName;
END;
GO
