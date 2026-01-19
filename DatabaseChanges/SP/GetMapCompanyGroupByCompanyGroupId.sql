
CREATE OR ALTER   PROC [dbo].[GetMapCompanyGroupByCompanyGroupId]
(
    @CompanyGroupId BIGINT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        MapCompanyGroupId,
        CompanyGroupId,
        CompanyId
    FROM dbo.MapCompanyGroup
    WHERE CompanyGroupId = @CompanyGroupId
    ORDER BY MapCompanyGroupId;
END
GO


