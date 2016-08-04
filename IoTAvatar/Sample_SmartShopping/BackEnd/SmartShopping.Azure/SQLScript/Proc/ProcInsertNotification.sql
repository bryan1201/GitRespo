CREATE PROCEDURE ProcInsertNotification
@id bigint
AS 
BEGIN

    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Notifications]
           ([AdvertisementId],[SendTime])
    VALUES
           (@id, GETUTCDATE());
END