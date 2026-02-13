CREATE PROCEDURE SP_ImportChequeEntry
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

    BEGIN TRY
        BEGIN TRAN;

        -------------------------------------------------
        -- 1️ DUPLICATE CHECK (WITH NAME RETURN)
        -------------------------------------------------
     
IF EXISTS
(
    SELECT 1
    FROM @ChequeList L
    WHERE EXISTS
    (
        SELECT 1
        FROM ChequeEntry C
        WHERE C.ChequeNo = L.ChequeNo
          AND CAST(C.ChequeEntryDate AS DATE) =
              CAST(L.ChequeEntryDate AS DATE)
          AND C.BankID = L.BankID
          AND C.CompanyID = L.CompanyID
          AND C.AccountID = L.AccountID
    )
)
BEGIN

    SELECT DISTINCT
        L.ChequeNo,

        FORMAT(L.ChequeEntryDate,'dd/MM/yyyy')
            AS ChequeDate,

        B.BankName,
        CM.CompanyName,
        A.AccountName

    FROM @ChequeList L

    -- 🔹 JOINs MUST COME BEFORE WHERE
    JOIN BankMaster B
        ON B.BankID = L.BankID

    JOIN CompanyMaster CM
        ON CM.CompanyID = L.CompanyID

    JOIN AccountMaster A
        ON A.AccountID = L.AccountID

    WHERE EXISTS
    (
        SELECT 1
        FROM ChequeEntry C
        WHERE C.ChequeNo = L.ChequeNo
          AND CAST(C.ChequeEntryDate AS DATE) =
              CAST(L.ChequeEntryDate AS DATE)
          AND C.BankID = L.BankID
          AND C.CompanyID = L.CompanyID
          AND C.AccountID = L.AccountID
    );

    RAISERROR(
        'Duplicate cheque entries found.',
        16,1);

    ROLLBACK;
    RETURN;
END


        -------------------------------------------------
        -- 2️⃣ CREATE IMPORT LOG
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
        SELECT
            @CompanyID,
            @BankID,
            COUNT(*),
            @UserID,
            @FileName,
            GETDATE()
        FROM @ChequeList;

        SET @ImportLogID = SCOPE_IDENTITY();

        -------------------------------------------------
        -- 3️⃣ TEMP TABLE FOR LOOP INSERT
        -------------------------------------------------
        DECLARE @Temp TABLE
        (
            RowNo INT IDENTITY(1,1),
            ChequeEntryDate DATETIME,
            ProjectID BIGINT,
            AccountID BIGINT,
            AccountSubName NVARCHAR(100),
            BankID BIGINT,
            ChequeNo NVARCHAR(20),
            TypeID BIGINT,
            SubTypeID BIGINT,
            ParameterID BIGINT,
            ChequeAmount DECIMAL(18,2),
            CompanyID BIGINT
        );

        INSERT INTO @Temp
        SELECT *
        FROM @ChequeList;

        -------------------------------------------------
        -- 4️⃣ LOOP INSERT (Running Code)
        -------------------------------------------------
        DECLARE
            @i INT = 1,
            @max INT,
            @NewNo NVARCHAR(20),
            @ChequeEntryCode NVARCHAR(20);

        SELECT @max = COUNT(*) FROM @Temp;

        WHILE @i <= @max
        BEGIN
            -- Running Code
            EXEC SP_GetNewNo
                 'TRN',
                 @NewNo OUTPUT;

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
                ChequeEntryDate,
                @ChequeEntryCode,
                ProjectID,
                AccountID,
                AccountSubName,
                BankID,
                ChequeNo,
                TypeID,
                SubTypeID,
                ParameterID,
                1,              -- ChequeTypeID
                1,              -- StatusID
                ChequeEntryDate,
                NULL,
                ChequeAmount,
                0,
                '',
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

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;

        DECLARE @Msg NVARCHAR(MAX) =
            ERROR_MESSAGE();

        RAISERROR(@Msg,16,1);
    END CATCH
END