IF OBJECT_ID('dbo.CreateMapCompanyGroup', 'P') IS NOT NULL
    DROP PROCEDURE dbo.CreateMapCompanyGroup;
GO

CREATE PROC [dbo].[CreateMapCompanyGroup]
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
        DECLARE @SameGroupMessage NVARCHAR(4000);

        SET @ConflictMessage = '';
        SET @SameGroupMessage = '';

        /* ===============================
           1. CHECK CONFLICTS (OTHER GROUPS)
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
           2. SAME GROUP CHECK
           =============================== */

        SELECT
            @SameGroupMessage =
                STUFF((
                    SELECT
                        CHAR(13) + CHAR(10) + '  - ' + CM.CompanyName
                    FROM dbo.MapCompanyGroup M
                    INNER JOIN @CompanyIds C
                        ON M.CompanyId = C.CompanyId
                    INNER JOIN dbo.CompanyMaster CM
                        ON CM.CompanyID = M.CompanyId
                    WHERE M.CompanyGroupId = @CompanyGroupId
                    FOR XML PATH(''), TYPE
                ).value('.', 'NVARCHAR(MAX)'), 1, 2, '');

        /* ===============================
           3. INSERT NEW MAPPINGS
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

        /* ===============================
           4. FINAL MESSAGE
           =============================== */

        IF (@SameGroupMessage IS NOT NULL AND LEN(@SameGroupMessage) > 0)
        BEGIN
            SET @Message =
                'Saved successfully.' +
                CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) +
                'The following companies were already in this group:' +
                CHAR(13) + CHAR(10) +
                @SameGroupMessage;
        END
        ELSE
        BEGIN
            SET @Message = 'SAVE';
        END

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Message = ERROR_MESSAGE();
    END CATCH
END;
GO
