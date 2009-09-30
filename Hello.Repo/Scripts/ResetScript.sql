DELETE FROM dbo.Tags 

DELETE FROM dbo.Users

DELETE FROM dbo.TideMarks

DELETE FROM dbo.QueuedTweets

DBCC CHECKIDENT ('dbo.TideMarks', RESEED, 0)

DBCC CHECKIDENT ('dbo.QueuedTweets', RESEED, 0)