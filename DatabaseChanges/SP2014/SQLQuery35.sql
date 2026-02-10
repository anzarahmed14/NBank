exec GetChequeReport @DateType=N'CCD',@StartDate='2002-01-01 00:00:00',@EndDate='2002-01-30 00:00:00',@ChequeStatusID=5,@ChequeTypeID=1,@UserId=1


exec "GetChequeReport";1 N'CCD', N'2002-01-01 00:00:00', N'2002-01-30 00:00:00', 5, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, N'', 1

exec "NBank"."dbo"."GetChequeReport";1 N'CCD', N'2022-01-01', N'2022-01-30', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'', 1

exec "NBank"."dbo"."GetChequeReport";1 N'CCD', N'2022-01-01', N'2022-01-30', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'', 1