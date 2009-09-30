SET NOCOUNT ON

DECLARE
	@max AS INT,
	@rc AS INT
	
SET @max = 1000000
SET @rc = 1

BEGIN TRAN
	INSERT INTO dbo.Numbers(Number) VALUES (1)
	
	WHILE @rc * 2 <= @max
	BEGIN
		INSERT INTO dbo.Numbers(number)
		SELECT Number + @rc
		FROM dbo.Numbers
		
		SET @rc = @rc * 2
	END
	
	INSERT INTO dbo.Numbers(Number)
	SELECT Number + @rc
	FROM dbo.Numbers
	WHERE Number + @rc <= @max
	
COMMIT TRAN
