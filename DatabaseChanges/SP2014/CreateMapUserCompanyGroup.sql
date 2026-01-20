IF OBJECT_ID('dbo.CreateMapUserCompanyGroup', 'P') IS NOT NULL
    DROP PROCEDURE dbo.CreateMapUserCompanyGroup;
GO

CREATE PROC [dbo].[CreateMapUserCompanyGroup]
(
    @UserId BIGINT,
    @CompanyGroupIds dbo.CompanyGroupIdList READONLY,
    @Message NVARCHAR(4000) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        /* DELETE unselected groups */
        DELETE M
        FROM dbo.MapUserCompanyGroup M
        WHERE M.UserId = @UserId
          AND NOT EXISTS
          (
              SELECT 1
              FROM @CompanyGroupIds C
              WHERE C.CompanyGroupId = M.CompanyGroupId
          );

        /* INSERT newly selected groups */
        INSERT INTO dbo.MapUserCompanyGroup (UserId, CompanyGroupId)
        SELECT 
            @UserId,
            C.CompanyGroupId
        FROM @CompanyGroupIds C
        WHERE NOT EXISTS
        (
            SELECT 1
            FROM dbo.MapUserCompanyGroup M
            WHERE M.UserId = @UserId
              AND M.CompanyGroupId = C.CompanyGroupId
        );

        COMMIT TRANSACTION;
        SET @Message = 'SAVE';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Message = ERROR_MESSAGE();
    END CATCH
END;
GO
