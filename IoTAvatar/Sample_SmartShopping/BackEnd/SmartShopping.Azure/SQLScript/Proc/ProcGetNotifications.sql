CREATE PROCEDURE ProcGetNotifications
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT Top 200 Advertisements.*
	FROM Advertisements
	JOIN Notifications
	ON Advertisements.Id=Notifications.AdvertisementId
	ORDER BY Advertisements.Id DESC;
END