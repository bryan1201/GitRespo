CREATE PROCEDURE ProcGetAdvertisementListByTime
@lastTime datetime
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT Top 100 *
	FROM [Advertisements]
	WHERE [Timestamp] > @lastTime 
        -- AND [Timestamp] < (CURRENT_TIMESTAMP - '00:00:02') -- Avoid lose data
	ORDER BY [Timestamp] ASC;
END