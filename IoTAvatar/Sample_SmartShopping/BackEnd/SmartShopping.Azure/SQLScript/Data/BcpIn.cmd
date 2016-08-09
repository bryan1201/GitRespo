setlocal

set SQLSERVER=avatar.database.windows.net
set SQLUSER=avatardbo
set SQLPASS=Avatar+123
set SQLDB=Avatar

bcp "%SQLDB%.dbo.Stores" in Data_Stores.txt -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c
bcp "%SQLDB%.dbo.Beacons" in Data_Beacons.txt -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c
bcp "%SQLDB%.dbo.Products" in Data_Products.txt -S -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c
bcp "%SQLDB%.dbo.BuyInfoes" in Data_BuyInfoes.txt -S -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c
REM bcp "%SQLDB%.dbo.Devices" in Data_Devices.txt -S -S %SQLSERVER% -U %SQLUSER%@%SQLSERVER% -P "%SQLPASS%" -c

endlocal