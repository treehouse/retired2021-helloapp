CREATE TABLE [dbo].[Friendships]
(
[Befriender] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[Befriendee] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Friendships] ADD CONSTRAINT [FK_Friendships_Users_Befriendee] FOREIGN KEY ([Befriendee]) REFERENCES [dbo].[Users] ([Username])
GO
ALTER TABLE [dbo].[Friendships] ADD CONSTRAINT [FK_Friendships_Users_Befriender] FOREIGN KEY ([Befriender]) REFERENCES [dbo].[Users] ([Username])
GO
