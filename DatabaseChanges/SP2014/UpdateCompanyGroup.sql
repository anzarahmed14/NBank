IF OBJECT_ID('dbo.UpdateCompanyGroup', 'P') IS NOT NULL
    DROP PROCEDURE dbo.UpdateCompanyGroup;
GO

CREATE PROC [dbo].[UpdateCompanyGroup]
(
    @Message NVARCHAR(4000) OUTPUT,
    @CompanyGroupID BIGINT,
    @CompanyGroupCode NVARCHAR(20) = NULL,
    @CompanyGroupName NVARCHAR(100) = NULL,
    @IsActive BIT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        /* DUPLICATE CHECK (excluding current record) */
        IF NOT EXISTS
        (
            SELECT 1
            FROM CompanyGroupMaster
            WHERE CompanyGroupName = @CompanyGroupName
              AND CompanyGroupID <> @CompanyGroupID
        )
        BEGIN
            UPDATE CompanyGroupMaster
            SET
                CompanyGroupCode = @CompanyGroupCode,
                CompanyGroupName = @CompanyGroupName,
                IsActive = @IsActive
            WHERE CompanyGroupID = @CompanyGroupID;

            SET @Message = 'SAVE';
            COMMIT TRANSACTION;
        END
        ELSE
        BEGIN
            SET @Message = 'DUPLICATE';
            ROLLBACK TRANSACTION;
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @Message = ERROR_MESSAGE();
    END CATCH
END;
GO
