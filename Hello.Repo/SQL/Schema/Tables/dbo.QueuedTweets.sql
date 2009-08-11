CREATE TABLE [dbo].[QueuedTweets]
(
[Username] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[Message] [nvarchar] (140) COLLATE Latin1_General_CI_AS NOT NULL,
[Created] [datetime] NOT NULL,
[Processed] [bit] NOT NULL CONSTRAINT [DF_QueuedTweets_Processed] DEFAULT ((0))
)
GO
