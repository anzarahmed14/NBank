CREATE PROCEDURE SP_GetImportLogList
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IL.ImportLogID,

        IL.CompanyID,
        CM.CompanyName,

        IL.BankID,
        BM.BankName,

        IL.TotalRows,
        IL.FileName,

        IL.CreatedUserID,
        UM.UserName AS CreatedUserName,

        IL.CreatedDate

    FROM ImportLog IL

    LEFT JOIN UserMaster UM
        ON UM.UserID = IL.CreatedUserID

    LEFT JOIN CompanyMaster CM
        ON CM.CompanyID = IL.CompanyID

    LEFT JOIN BankMaster BM
        ON BM.BankID = IL.BankID

    ORDER BY IL.ImportLogID DESC;
END
GO