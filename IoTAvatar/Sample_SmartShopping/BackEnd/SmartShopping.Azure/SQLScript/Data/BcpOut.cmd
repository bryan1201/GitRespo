setlocal

set SQLSERVER=(server hostname)
set SQLUSER=(sql user account)
set SQLPASS=(sql user account password)
set SQLDB=(db name)

bcp %SQLDB%.dbo.Beacons out Data_Beacons.txt -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c
bcp %SQLDB%.dbo.BuyInfoes out Data_BuyInfoes.txt -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c
bcp %SQLDB%.dbo.Stores out Data_Stores.txt -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c
bcp %SQLDB%.dbo.Products out Data_Products.txt -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c
REM bcp %SQLDB%.dbo.Devices out Data_Devices.txt -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c

endlocal