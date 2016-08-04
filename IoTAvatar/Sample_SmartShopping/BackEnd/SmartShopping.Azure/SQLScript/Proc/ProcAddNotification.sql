CREATE PROCEDURE ProcAddNotification
@beacon nvarchar(50),
@device nvarchar(50)
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Advertisements]
           ([BeaconId],[TargetDeviceId],[SignalStrength],[Timestamp],[CreatedTime])
    VALUES
           (@beacon,@device,-126, GETUTCDATE(), GETUTCDATE());

    INSERT INTO [dbo].[Advertisements]
           ([BeaconId],[TargetDeviceId],[SignalStrength],[Timestamp],[CreatedTime])
    VALUES
           (@beacon,@device,-16, GETUTCDATE(), GETUTCDATE());
END