
ALTER Procedure [dbo].[CreateChequeEntry]
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
,@CreatedDate Datetime = null
,@UpdatedDate Datetime  =  null
AS
BEGIN
BEGIN TRANSACTION
	BEGIN TRY
		/*Check duplicate cheque no*/
		IF EXISTS(SELECT * FROM ChequeEntry WHERE BankID=@BankID AND  CompanyID=@CompanyID AND ChequeNo=@ChequeNo)
		BEGIN
				SET @Message='DUCH'
				ROLLBACK TRANSACTION
			RETURN	
		END 

		/*Get New Code No*/
		DECLARE @NewNo NVARCHAR(20) 
		EXEC SP_GetNewNo 'TRN' ,@NewNo OUTPUT

		/*Check New No*/
		IF (LEN(@NewNo)=0 OR @NewNo IS NULL )
			BEGIN
				SET @Message='CODE'
				ROLLBACK TRANSACTION
				RETURN
			END
		/*Set New Number TO Code*/
		SET @ChequeEntryCode  = @NewNo
		INSERT INTO ChequeEntry(ChequeEntryDate,ChequeEntryCode,ProjectID,AccountID,AccountSubName,BankID,ChequeNo,TypeID,SubTypeID,ParameterID,ChequeTypeID,ChequeStatusID,ChequeIssueDate,ChequeClearDate,ChequeAmount,ChequeAmountTDS,Narration,CompanyID,CreatedUserID,UpdatedUserID,ERPID)VALUES(@ChequeEntryDate,@ChequeEntryCode,@ProjectID,@AccountID,@AccountSubName,@BankID,@ChequeNo,@TypeID,@SubTypeID,@ParameterID,@ChequeTypeID,@ChequeStatusID,@ChequeIssueDate,@ChequeClearDate,@ChequeAmount,@ChequeAmountTDS,@Narration,@CompanyID,@CreatedUserID,@UpdatedUserID,@ERPID)
		

		DECLARE @NewChequeNo NVARCHAR(20)
		SET @NewChequeNo =( SELECT ISNUMERIC(@ChequeNo))
		/*First Cheque ISSUE*/
		IF (@ChequeTypeID=1 AND @NewChequeNo = 1 )
			BEGIN
				IF EXISTS(SELECT * FROM CompanyWiseBankMaster WHERE BankID  = @BankID AND  CompanyID = @CompanyID)
					BEGIN
				
						UPDATE CompanyWiseBankMaster SET LastChequeNo = (@ChequeNo +1), LastChequeEntryDate = CONVERT(VARCHAR(26),GETDATE(), 23)  WHERE BankID  = @BankID AND  CompanyID  = @CompanyID 
					END
				ELSE
					BEGIN
						SET @NewNo = ''
						EXEC SP_GetNewNo 'CWB' ,@NewNo OUTPUT

						/*Check New No*/
						IF (LEN(@NewNo)=0 OR @NewNo IS NULL )
							BEGIN
								SET @Message='CODE'
								ROLLBACK TRANSACTION
								RETURN
							END
						INSERT INTO CompanyWiseBankMaster(BankID,CompanyID,LastChequeNo,LastChequeEntryDate,CompanyWiseBankCode)VALUES(@BankID,@CompanyID,(@ChequeNo+1),  CONVERT(VARCHAR(26),GETDATE(), 23),@NewNo)
					END 

			END
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


