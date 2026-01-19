
CREATE OR ALTER   PROC [dbo].[UpdateCompanyGroup]
@Message  NVARCHAR(4000) OUTPUT
,@CompanyGroupID bigint
,@CompanyGroupCode nvarchar(20)  = null
,@CompanyGroupName nvarchar(100)  = null
,@IsActive bit= null
AS
BEGIN
BEGIN TRANSACTION
    BEGIN TRY
		IF NOT EXISTS(SELECT * FROM CompanyGroupMaster WHERE CompanyGroupName=@CompanyGroupName AND CompanyGroupID NOT IN (@CompanyGroupID))
			BEGIN
				 UPDATE CompanyGroupMaster SET 
				CompanyGroupCode = @CompanyGroupCode,
				CompanyGroupName = @CompanyGroupName,
				IsActive = @IsActive 
			WHERE CompanyGroupID =@CompanyGroupID
			IF @@ERROR = 0 
				BEGIN
					SET @Message='SAVE'
					COMMIT TRANSACTION
				END
			END
		ELSE
			BEGIN
				SET @Message='DUPLICATE'
				ROLLBACK TRANSACTION
			END
      END TRY
    BEGIN CATCH
        SET  @Message=ERROR_MESSAGE() 
        ROLLBACK TRANSACTION
    END CATCH
END

GO


