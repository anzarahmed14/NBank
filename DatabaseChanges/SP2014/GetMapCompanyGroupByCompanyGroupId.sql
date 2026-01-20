IF OBJECT_ID('dbo.GetMapCompanyGroupByCompanyGroupId', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetMapCompanyGroupByCompanyGroupId;
GO

CREATE PROC [dbo].[GetMapCompanyGroupByCompanyGroupId]
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
END;
GO
