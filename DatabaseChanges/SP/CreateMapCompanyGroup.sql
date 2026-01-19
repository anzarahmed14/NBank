
CREATE OR ALTER   PROC [dbo].[CreateMapCompanyGroup]
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

        DECLARE @ConflictMessage NVARCHAR(4000) = '';
        DECLARE @SameGroupMessage NVARCHAR(4000) = '';

      
        ;WITH ConflictPerGroup AS
        (
            SELECT
                CG.CompanyGroupName,
                STRING_AGG('  - ' + CM.CompanyName, CHAR(13) + CHAR(10)) 
                    AS CompanyList
            FROM dbo.MapCompanyGroup M
            INNER JOIN @CompanyIds C
                ON M.CompanyId = C.CompanyId
            INNER JOIN dbo.CompanyMaster CM
                ON CM.CompanyID = M.CompanyId
            INNER JOIN dbo.CompanyGroupMaster CG
                ON CG.CompanyGroupID = M.CompanyGroupId
            WHERE M.CompanyGroupId <> @CompanyGroupId
            GROUP BY CG.CompanyGroupName
        )
        SELECT
            @ConflictMessage =
                STRING_AGG(
                    'Group ' + CompanyGroupName + ' :' 
                    + CHAR(13) + CHAR(10) 
                    + CompanyList,
                    CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10)
                )
        FROM ConflictPerGroup;

        IF (@ConflictMessage IS NOT NULL AND LEN(@ConflictMessage) > 0)
        BEGIN
            SET @Message =
                'The following companies are already assigned to other groups:' 
                + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10)
                + @ConflictMessage;

            ROLLBACK TRANSACTION;
            RETURN;
        END

      
        ;WITH SameGroupCompanies AS
        (
            SELECT
                CM.CompanyName
            FROM dbo.MapCompanyGroup M
            INNER JOIN @CompanyIds C
                ON M.CompanyId = C.CompanyId
            INNER JOIN dbo.CompanyMaster CM
                ON CM.CompanyID = M.CompanyId
            WHERE M.CompanyGroupId = @CompanyGroupId
        )
        SELECT
            @SameGroupMessage =
                STRING_AGG('  - ' + CompanyName, CHAR(13) + CHAR(10))
        FROM SameGroupCompanies;

      
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

     
        IF (@SameGroupMessage IS NOT NULL AND LEN(@SameGroupMessage) > 0)
        BEGIN
            SET @Message =
                'Saved successfully.' 
                + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10)
                + 'The following companies were already in this group:' 
                + CHAR(13) + CHAR(10)
                + @SameGroupMessage;
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
END
GO


