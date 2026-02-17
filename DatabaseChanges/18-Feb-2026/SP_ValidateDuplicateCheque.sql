CREATE PROCEDURE SP_ValidateDuplicateCheque
(
    @ChequeList dbo.ChequeEntryImportType READONLY
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        L.ChequeNo,
        L.IssueDate,
        L.BankID,
        L.CompanyID
    FROM @ChequeList L
    WHERE EXISTS
    (
        SELECT 1
        FROM ChequeEntry C
        WHERE C.ChequeNo = L.ChequeNo
          AND CAST(C.ChequeIssueDate AS DATE) =
              CAST(L.IssueDate AS DATE)
          AND C.BankID = L.BankID
          AND C.CompanyID = L.CompanyID
    );
END
