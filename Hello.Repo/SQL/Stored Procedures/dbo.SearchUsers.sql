
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[SearchUsers] 
	-- Add the parameters for the stored procedure here
	@SearchString VARCHAR(MAX),
	@MaxResults INTEGER
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	CREATE TABLE #SearchTerms (
		Term VARCHAR(MAX) COLLATE Latin1_General_CI_AS
	)
	
	INSERT INTO #SearchTerms ( Term )
	SELECT element
	FROM dbo.SplitString(@SearchString, ' ')
	
	CREATE TABLE #TempResults (
		Username VARCHAR(50) COLLATE Latin1_General_CI_AS,
		Weighting INTEGER)

	-- Strongly weight exact match of username
	INSERT INTO #TempResults ( Username, Weighting )
	SELECT u.Username, 10
	FROM dbo.Users u
	INNER JOIN #SearchTerms t ON u.Username = t.Term
	WHERE u.ShadowAccount = 0
	
	-- Then LIKE match of username
	INSERT INTO #TempResults (Username, Weighting)
	SELECT u.Username, 10
	FROM dbo.Users u
	INNER JOIN #SearchTerms t ON u.Username LIKE '%' + t.Term + '%' AND u.Username <> t.Term
	WHERE u.ShadowAccount = 0
	
	-- Match on usertypes (start with a raw match)
	INSERT INTO #TempResults ( Username, Weighting )
	SELECT u.Username, 10
	FROM dbo.Users u
	INNER JOIN dbo.UserTypes ut ON u.UserTypeID = ut.UserTypeID
	INNER JOIN #SearchTerms t ON ut.Name = t.Term
	WHERE u.ShadowAccount = 0
	
	-- Match on usertypes where explicit usertype (eg !dev)
	INSERT INTO #TempResults ( Username, Weighting )
	SELECT u.Username, 10
	FROM dbo.Users u
	INNER JOIN dbo.UserTypes ut ON u.UserTypeID = ut.UserTypeID
	INNER JOIN #SearchTerms t ON ut.Name = RIGHT(t.Term, LEN(t.Term) - 1)
	WHERE u.ShadowAccount = 0 AND LEFT(t.Term, 1) = '!'
	
	-- Raw match of tags
	INSERT INTO #TempResults (Username, Weighting)
	SELECT u.Username, 10
	FROM dbo.Users u
	INNER JOIN dbo.Tags t ON u.Username = t.Username
	INNER JOIN #SearchTerms st ON t.Name = st.Term
	WHERE u.ShadowAccount = 0
	
	-- Match on tags where explicit tag (eg #mvc)
	INSERT INTO #TempResults (Username, Weighting)
	SELECT u.Username, 10
	FROM dbo.Users u
	INNER JOIN dbo.Tags t ON u.Username = t.Username
	INNER JOIN #SearchTerms st ON t.Name = RIGHT(st.Term, LEN(st.Term) - 1)
	WHERE u.ShadowAccount = 0 AND LEFT(st.Term, 1) = '#'

    -- Insert statements for procedure here
	SELECT u.Username, u.ImageURL, u.Created, u.Updated, u.UserTypeID, u.ShadowAccount, u.Followers
	FROM dbo.Users u
	INNER JOIN #TempResults t ON u.Username = t.Username
	GROUP BY u.Username, u.ImageURL, u.Created, u.Updated, u.UserTypeID, u.ShadowAccount, u.Followers
	HAVING SUM(t.Weighting) >= (SELECT 10 * COUNT(*) FROM #SearchTerms)
	ORDER BY SUM(t.Weighting) DESC
	
	DROP TABLE #TempResults
	DROP TABLE #SearchTerms
END
GO




GRANT EXECUTE ON  [dbo].[SearchUsers] TO [hello_web]
GO
