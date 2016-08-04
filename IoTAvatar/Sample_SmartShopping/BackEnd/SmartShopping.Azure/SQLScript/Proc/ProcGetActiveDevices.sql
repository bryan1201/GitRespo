CREATE PROCEDURE ProcGetActiveDevices
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT Top 2 DeviceId
	FROM Devices
	WHERE IsActive = 1
	ORDER BY DevId DESC
END