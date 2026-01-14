USE [NBank]
GO

/****** Object:  StoredProcedure [dbo].[GetCompanyGroup]    Script Date: 14-01-2026 11:47:40 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROC [dbo].[GetCompanyGroup]
@CompanyGroupID bigint= null
,@CompanyGroupName NVARCHAR(100)= null
AS
BEGIN
	IF (LEN(@CompanyGroupName)>0 OR @CompanyGroupName IS NOT NULL )
		BEGIN
			SELECT *, CONCAT(CompanyGroupName, ' ', CompanyGroupCode) as 'FULL_NAME' FROM CompanyGroupMaster WHERE CONCAT(CompanyGroupName, ' ', CompanyGroupCode)  like '%'+@CompanyGroupName+'%' ORDER BY CompanyGroupName
		END
	ELSE
		BEGIN
			SELECT * FROM CompanyGroupMaster WHERE CompanyGroupID=ISNULL(@CompanyGroupID,CompanyGroupID) ORDER BY CompanyGroupName
		END
END


GO


