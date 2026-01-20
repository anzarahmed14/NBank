IF OBJECT_ID('dbo.UpdateMapCompanyGroup', 'P') IS NOT NULL
    DROP PROCEDURE dbo.UpdateMapCompanyGroup;
GO

CREATE PROC [dbo].[UpdateMapCompanyGroup]
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

        DECLARE @ConflictMessage NVARCHAR(4000);
        SET @ConflictMessage = '';

        /* ===============================
           1. COLLECT CONFLICT DATA
           =============================== */
        ;WITH ConflictData AS
        (
            SELECT
                CG.CompanyGroupName,
                CM.CompanyName
            FROM dbo.MapCompanyGroup M
            INNER JOIN @CompanyIds C
                ON M.CompanyId = C.CompanyId
            INNER JOIN dbo.CompanyMaster CM
                ON CM.CompanyID = M.CompanyId
            INNER JOIN dbo.CompanyGroupMaster CG
                ON CG.CompanyGroupID = M.CompanyGroupId
            WHERE M.CompanyGroupId <> @CompanyGroupId
        )
        SELECT
            @ConflictMessage =
                STUFF((
                    SELECT
                        CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) +
                        'Group ' + CD.CompanyGroupName + ' :' +
                        CHAR(13) + CHAR(10) +
                        STUFF((
                            SELECT
                                CHAR(13) + CHAR(10) + '  - ' + CD2.CompanyName
                            FROM ConflictData CD2
                            WHERE CD2.CompanyGroupName = CD.CompanyGroupName
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
                    FROM
                        (SELECT DISTINCT CompanyGroupName FROM ConflictData) CD
                    FOR XML PATH(''), TYPE
                ).value('.', 'NVARCHAR(MAX)'), 1, 4, '');

        /* ===============================
           2. STOP IF CONFLICT EXISTS
           =============================== */
        IF (@ConflictMessage IS NOT NULL AND LEN(@ConflictMessage) > 0)
        BEGIN
            SET @Message =
                'The following companies are already assigned to other groups:' +
                CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) +
                @ConflictMessage;

            ROLLBACK TRANSACTION;
            RETURN;
        END

        /* ===============================
           3. DELETE UNCHECKED COMPANIES
           =============================== */
        DELETE M
        FROM dbo.MapCompanyGroup M
        WHERE M.CompanyGroupId = @CompanyGroupId
          AND NOT EXISTS
          (
              SELECT 1
              FROM @CompanyIds C
              WHERE C.CompanyId = M.CompanyId
          );

        /* ===============================
           4. INSERT NEW COMPANIES
           =============================== */
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
END;
GO
