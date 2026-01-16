CREATE OR ALTER PROC dbo.GetMapUserCompanyGroupByUserId
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
END
GO


EXEC GetMapUserCompanyGroupByUserId 30005

--MapUserCompanyGroupId	UserId	CompanyGroupId	CompanyGroupName
4	30005	2	A

SELECT * FROM MapUserCompanyGroup