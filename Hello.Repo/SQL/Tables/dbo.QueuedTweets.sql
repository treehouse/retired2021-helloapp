CREATE TABLE [dbo].[QueuedTweets]
(
[QueuedTweetID] [int] NOT NULL IDENTITY(1, 1),
[Username] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[Message] [nvarchar] (140) COLLATE Latin1_General_CI_AS NOT NULL,
[Created] [datetime] NOT NULL,
[Processed] [bit] NOT NULL CONSTRAINT [DF_QueuedTweets_Processed] DEFAULT ((0))
)

ALTER TABLE [dbo].[QueuedTweets] ADD 
CONSTRAINT [PK_QueuedTweets] PRIMARY KEY CLUSTERED  ([QueuedTweetID])
GO
