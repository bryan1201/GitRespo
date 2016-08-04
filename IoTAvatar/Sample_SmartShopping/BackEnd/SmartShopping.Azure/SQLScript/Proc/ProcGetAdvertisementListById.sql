CREATE PROCEDURE ProcGetAdvertisementListById
@id bigint
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT Top 100 *
	FROM [Advertisements]
	WHERE [Id] > @id 
	ORDER BY [Timestamp] ASC;
END