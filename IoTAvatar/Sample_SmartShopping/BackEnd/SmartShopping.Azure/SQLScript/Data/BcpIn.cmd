setlocal

set SQLSERVER=(server hostname)
set SQLUSER=(sql user account)
set SQLPASS=(sql user account password)
set SQLDB=(db name)

bcp "%SQLDB%.dbo.Stores" in Data_Stores.txt -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c
bcp "%SQLDB%.dbo.Beacons" in Data_Beacons.txt -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c
bcp "%SQLDB%.dbo.Products" in Data_Products.txt -S -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c
bcp "%SQLDB%.dbo.BuyInfoes" in Data_BuyInfoes.txt -S -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c
REM bcp "%SQLDB%.dbo.Devices" in Data_Devices.txt -S -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c

endlocal