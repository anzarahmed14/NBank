ALTER PROCEDURE [dbo].[SP_ImportChequeEntry]
(
    @ChequeList dbo.ChequeEntryImportType READONLY,
    @CompanyID BIGINT,
    @BankID BIGINT,
    @UserID BIGINT,
    @FileName NVARCHAR(200)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ImportLogID BIGINT;
    DECLARE @TotalRows INT;

    BEGIN TRY
        BEGIN TRAN;

        -------------------------------------------------
        -- 1️ COUNT ROWS
        -------------------------------------------------
        SELECT @TotalRows = COUNT(*)
        FROM @ChequeList;


        -------------------------------------------------
        -- 2️ CREATE IMPORT LOG
        -------------------------------------------------
        INSERT INTO ImportLog
        (
            CompanyID,
            BankID,
            TotalRows,
            CreatedUserID,
            FileName,
            CreatedDate
        )
        VALUES
        (
            @CompanyID,
            @BankID,
            @TotalRows,
            @UserID,
            @FileName,
            GETDATE()
        );

        SET @ImportLogID = SCOPE_IDENTITY();


        -------------------------------------------------
        -- 3️ TEMP TABLE
        -------------------------------------------------
        DECLARE @Temp TABLE
        (
            RowNo INT IDENTITY(1,1),
            EntryDate DATETIME,
            IssueDate DATETIME,
            ProjectID BIGINT,
            AccountID BIGINT,
            AccountSubName NVARCHAR(100),
            BankID BIGINT,
            ChequeNo NVARCHAR(20),
            TypeID BIGINT,
            SubTypeID BIGINT,
            ParameterID BIGINT,
            ChequeAmount DECIMAL(18,2),
            CompanyID BIGINT,
            Narration NVARCHAR(MAX)
        );

        INSERT INTO @Temp
        SELECT *
        FROM @ChequeList;


        -------------------------------------------------
        -- 4️ LOOP INSERT
        -------------------------------------------------
        DECLARE
            @i INT = 1,
            @max INT,
            @NewNo NVARCHAR(20),
            @ChequeEntryCode NVARCHAR(20);

        SELECT @max = COUNT(*) FROM @Temp;

        WHILE @i <= @max
        BEGIN
            EXEC SP_GetNewNo 'TRN', @NewNo OUTPUT;

            SET @ChequeEntryCode = @NewNo;

            INSERT INTO ChequeEntry
            (
                ChequeEntryDate,
                ChequeEntryCode,
                ProjectID,
                AccountID,
                AccountSubName,
                BankID,
                ChequeNo,
                TypeID,
                SubTypeID,
                ParameterID,
                ChequeTypeID,
                ChequeStatusID,
                ChequeIssueDate,
                ChequeClearDate,
                ChequeAmount,
                ChequeAmountTDS,
                Narration,
                CompanyID,
                CreatedUserID,
                UpdatedUserID,
                OldProjectID,
                OldCompanyID,
                OldAccountID,
                ERPID,
                ImportLogID
            )
            SELECT
                EntryDate,
                @ChequeEntryCode,
                ProjectID,
                AccountID,
                AccountSubName,
                BankID,
                ChequeNo,
                TypeID,
                SubTypeID,
                ParameterID,
                1,
                1,
                IssueDate,
                NULL,
                ChequeAmount,
                0,
                Narration,
                CompanyID,
                @UserID,
                @UserID,
                ProjectID,
                CompanyID,
                AccountID,
                NULL,
                @ImportLogID
            FROM @Temp
            WHERE RowNo = @i;

            SET @i += 1;
        END


        -------------------------------------------------
        -- 5️ COMMIT
        -------------------------------------------------
        COMMIT;


        -------------------------------------------------
        -- 6️ SUCCESS RESULT
        -------------------------------------------------
        SELECT
            1 AS Status,
            'Import completed successfully.' AS Message,
            @TotalRows AS InsertedRows,
            @ImportLogID AS ImportLogID;

    END TRY

    BEGIN CATCH
        ROLLBACK;

        SELECT
            0 AS Status,
            ERROR_MESSAGE() AS Message,
            0 AS InsertedRows,
            NULL AS ImportLogID;
    END CATCH
END
GO
