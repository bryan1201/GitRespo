CREATE PROCEDURE ProcCountNotification
@store nvarchar(50),
@product nvarchar(50)
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT Count(Notifications.AdvertisementId)
	FROM Advertisements
	JOIN Notifications ON Advertisements.Id=Notifications.AdvertisementId
	JOIN Beacons ON Advertisements.BeaconId=Beacons.BeaconId
	WHERE Beacons.StoreId=@store  
	AND Beacons.ProductId=@product;
END