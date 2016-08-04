CREATE PROCEDURE ProcProductList
@store nvarchar(50),
@product nvarchar(50)
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT Top 50 Advertisements.*
	FROM Advertisements
	JOIN Beacons ON Advertisements.BeaconId=Beacons.BeaconId 
	WHERE Beacons.StoreId=@store  
	AND Beacons.ProductId=@product
	ORDER BY Advertisements.Id DESC;
END