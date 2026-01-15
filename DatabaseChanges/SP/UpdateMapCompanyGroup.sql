CREATE OR ALTER PROC dbo.UpdateMapCompanyGroup
(
    @CompanyGroupId BIGINT,
    @CompanyIds dbo.CompanyIdList READONLY,
    @Message NVARCHAR(4000) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @ConflictCompanies NVARCHAR(2000) = '';

       
        SELECT 
            @ConflictCompanies = STRING_AGG(CM.CompanyName, ', ')
        FROM dbo.MapCompanyGroup M
        INNER JOIN @CompanyIds C
            ON M.CompanyId = C.CompanyId
        INNER JOIN dbo.CompanyMaster CM
            ON CM.CompanyID = M.CompanyId
        WHERE M.CompanyGroupId <> @CompanyGroupId;

        IF (@ConflictCompanies IS NOT NULL AND LEN(@ConflictCompanies) > 0)
        BEGIN
            SET @Message =
                'These companies are already assigned to another group: '
                + @ConflictCompanies;
            ROLLBACK TRANSACTION;
            RETURN;
        END

        /* 🔄 DELETE: companies unchecked by user */
        DELETE M
        FROM dbo.MapCompanyGroup M
        WHERE M.CompanyGroupId = @CompanyGroupId
          AND NOT EXISTS
          (
              SELECT 1
              FROM @CompanyIds C
              WHERE C.CompanyId = M.CompanyId
          );

        /* ➕ INSERT: newly selected companies */
        INSERT INTO dbo.MapCompanyGroup (CompanyGroupId, CompanyId)
        SELECT 
            @CompanyGroupId,
            C.CompanyId
        FROM @CompanyIds C
        WHERE NOT EXISTS
        (
            SELECT 1
            FROM dbo.MapCompanyGroup M
            WHERE M.CompanyGroupId = @CompanyGroupId
              AND M.CompanyId = C.CompanyId
        );

        COMMIT TRANSACTION;
        SET @Message = 'UPDATE';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO
