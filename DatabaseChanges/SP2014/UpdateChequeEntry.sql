IF OBJECT_ID('dbo.UpdateChequeEntry', 'P') IS NOT NULL
    DROP PROCEDURE dbo.UpdateChequeEntry;
GO

CREATE PROCEDURE [dbo].[UpdateChequeEntry]
@Message  NVARCHAR(4000) OUTPUT
,@ChequeEntryID BIGINT = null
,@ChequeEntryDate datetime= null
,@ChequeEntryCode nvarchar(20)  = null
,@ProjectID bigint= null
,@AccountID bigint= null
,@AccountSubName nvarchar(100)  = null
,@BankID bigint= null
,@ChequeNo nvarchar(20)  = null
,@TypeID bigint= null
,@SubTypeID bigint= null
,@ParameterID bigint= null
,@ChequeTypeID bigint= null
,@ChequeStatusID bigint= null
,@ChequeIssueDate date= null
,@ChequeClearDate date = null
,@ChequeAmount decimal(18,2)= null
,@ChequeAmountTDS decimal(18,2)= null
,@Narration nvarchar(MAX)  = null
,@CompanyID BIGINT =null
,@CreatedUserID BIGINT = null
,@UpdatedUserID BIGINT = null
,@ERPID NVARCHAR(MAX) = null
AS
BEGIN
BEGIN TRANSACTION
	/*Check duplicate cheque no*/
		IF EXISTS(SELECT * FROM ChequeEntry WHERE BankID=@BankID AND  CompanyID=@CompanyID AND ChequeNo=@ChequeNo AND ChequeEntryID NOT IN (@ChequeEntryID))
		BEGIN
				SET @Message='DUCH'
				ROLLBACK TRANSACTION
			RETURN	
		END 
    BEGIN TRY
        UPDATE ChequeEntry SET 
            --ChequeEntryID = @ChequeEntryID,
            ChequeEntryDate = @ChequeEntryDate,
           -- ChequeEntryCode = @ChequeEntryCode,
            ProjectID = @ProjectID,
            AccountID = @AccountID,
            AccountSubName = @AccountSubName,
            BankID = @BankID,
            ChequeNo = @ChequeNo,
            TypeID = @TypeID,
            SubTypeID = @SubTypeID,
            ParameterID = @ParameterID,
            ChequeTypeID = @ChequeTypeID,
            ChequeStatusID = @ChequeStatusID,
            ChequeIssueDate = @ChequeIssueDate,
            ChequeClearDate = @ChequeClearDate,
            ChequeAmount = @ChequeAmount,
            ChequeAmountTDS = @ChequeAmountTDS,
            Narration = @Narration,
            CompanyID = @CompanyID,
			UpdatedUserID = @UpdatedUserID,
			ERPID = @ERPID 
        WHERE ChequeEntryID =@ChequeEntryID
        IF @@ERROR = 0 
            BEGIN
                SET @Message='SAVE'
                COMMIT TRANSACTION
            END
      END TRY
    BEGIN CATCH
        SET  @Message=ERROR_MESSAGE() 
        ROLLBACK TRANSACTION
    END CATCH
END

GO


