SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION SplitString 
(	
	@arr AS VARCHAR(MAX),
	@sep AS CHAR(1)
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT
		(Number - 1) - LEN(REPLACE(LEFT(@arr, Number - 1), @sep, '')) + 1 AS pos,
		SUBSTRING(@arr, Number, CHARINDEX(@sep, @arr + @sep, Number) - Number) AS element
	FROM dbo.Numbers
	WHERE Number <= LEN(@arr) + 1
		AND SUBSTRING(@sep + @arr, Number, 1) = @sep

)
GO
