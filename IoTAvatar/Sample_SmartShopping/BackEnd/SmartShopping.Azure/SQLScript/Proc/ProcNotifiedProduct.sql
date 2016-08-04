CREATE PROCEDURE ProcNotifiedProduct
@store nvarchar(50)
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT Top 1 Beacons.ProductId
	FROM Advertisements
	JOIN Beacons ON Advertisements.BeaconId=Beacons.BeaconId
	JOIN Notifications ON Notifications.AdvertisementId=Advertisements.Id
	WHERE Beacons.StoreId=@store  
	ORDER BY Advertisements.Id DESC;
END