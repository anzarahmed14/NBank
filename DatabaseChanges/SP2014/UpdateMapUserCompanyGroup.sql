IF OBJECT_ID('dbo.UpdateMapUserCompanyGroup', 'P') IS NOT NULL
    DROP PROCEDURE dbo.UpdateMapUserCompanyGroup;
GO

CREATE PROC [dbo].[UpdateMapUserCompanyGroup]
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

        /* DELETE groups unchecked by user */
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
        SET @Message = 'UPDATE';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Message = ERROR_MESSAGE();
    END CATCH
END;
GO
