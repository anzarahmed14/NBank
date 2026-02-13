
ALTER VIEW [dbo].[View_ChequeEntry]
AS
SELECT        dbo.ChequeEntry.ChequeEntryID, dbo.ChequeEntry.ChequeEntryDate, dbo.ProjectMaster.ProjectName, dbo.AccountMaster.AccountName, dbo.CompanyMaster.CompanyID, dbo.CompanyMaster.CompanyName, 
                         dbo.CompanyMaster.CompanyCode, dbo.ProjectMaster.ProjectShortName, dbo.ProjectMaster.ProjectCode, dbo.ProjectMaster.ProjectID, dbo.BankMaster.BankID, dbo.BankMaster.BankName, dbo.BankMaster.BankCode, 
                         dbo.AccountMaster.AccountID, dbo.ChequeTypeMaster.ChequeTypeName, dbo.ChequeTypeMaster.ChequeTypeID, dbo.ChequeTypeMaster.ChequeTypeCode, dbo.ChequeEntry.ChequeNo, dbo.ChequeEntry.ChequeIssueDate, 
                         dbo.ChequeEntry.ChequeClearDate, dbo.ChequeEntry.ChequeAmount, dbo.ChequeStatusMaster.ChequeStatusShortName, dbo.ChequeStatusMaster.ChequeStatusName, dbo.ChequeStatusMaster.ChequeStatusID, 
                         dbo.ChequeEntry.AccountSubName, dbo.ChequeEntry.Narration, dbo.ChequeEntry.ChequeAmountTDS, dbo.ChequeEntry.SubTypeID, dbo.ChequeEntry.TypeID, dbo.ChequeEntry.ParameterID, dbo.SubTypeMaster.SubTypeName, 
                         dbo.SubTypeMaster.SubTypeShortName, dbo.SubTypeMaster.SubTypeCode, dbo.TypeMaster.TypeShortName, dbo.TypeMaster.TypeCode, dbo.TypeMaster.TypeName, dbo.ParameterMaster.ParameterCode, 
                         dbo.ParameterMaster.ParameterName, dbo.ParameterMaster.ParameterShortName, ISNULL(dbo.AccountMaster.OpeningBalance, 0) AS OpeningBalance, dbo.CompanyMaster.CompanyShortName, 
                         dbo.AccountMaster.AccountShortName, dbo.ChequeEntry.ERPID, dbo.ChequeEntry.CreatedDate, dbo.ChequeEntry.UpdatedDate, dbo.UserMaster.UserName AS UpdatedUserName, 
                         UserMaster_1.UserName AS CreatedUserName
FROM            dbo.ProjectMaster RIGHT OUTER JOIN
                         dbo.ChequeEntry LEFT OUTER JOIN
                         dbo.UserMaster ON dbo.ChequeEntry.UpdatedUserID = dbo.UserMaster.UserID LEFT OUTER JOIN
                         dbo.UserMaster AS UserMaster_1 ON dbo.ChequeEntry.CreatedUserID = UserMaster_1.UserID LEFT OUTER JOIN
                         dbo.ParameterMaster ON dbo.ChequeEntry.ParameterID = dbo.ParameterMaster.ParameterID LEFT OUTER JOIN
                         dbo.TypeMaster ON dbo.ChequeEntry.TypeID = dbo.TypeMaster.TypeID LEFT OUTER JOIN
                         dbo.SubTypeMaster ON dbo.ChequeEntry.SubTypeID = dbo.SubTypeMaster.SubTypeID LEFT OUTER JOIN
                         dbo.ChequeStatusMaster ON dbo.ChequeEntry.ChequeStatusID = dbo.ChequeStatusMaster.ChequeStatusID LEFT OUTER JOIN
                         dbo.AccountMaster ON dbo.ChequeEntry.AccountID = dbo.AccountMaster.AccountID LEFT OUTER JOIN
                         dbo.ChequeTypeMaster ON dbo.ChequeEntry.ChequeTypeID = dbo.ChequeTypeMaster.ChequeTypeID LEFT OUTER JOIN
                         dbo.CompanyMaster ON dbo.ChequeEntry.CompanyID = dbo.CompanyMaster.CompanyID LEFT OUTER JOIN
                         dbo.BankMaster ON dbo.ChequeEntry.BankID = dbo.BankMaster.BankID ON dbo.ProjectMaster.ProjectID = dbo.ChequeEntry.ProjectID
GO


