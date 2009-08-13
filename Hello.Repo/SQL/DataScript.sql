/*
Run this script on a database with the same schema as:

C:\dev\BlankDB\Data – the database with this schema will be modified

to synchronize its data with:

RYAN\SQLEXPRESS.helloapp – this database will not be modified

You are recommended to back up your database before running this script

Script created by SQL Data Compare version 8.0.1 from Red Gate Software Ltd at 13/08/2009 20:49:05

*/
			
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
GO
SET DATEFORMAT YMD
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
-- Pointer used for text / image updates. This might not be needed, but is declared here just in case
DECLARE @pv binary(16)
BEGIN TRANSACTION

-- Drop constraints from [dbo].[UserTypes]
ALTER TABLE [dbo].[UserTypes] DROP CONSTRAINT [FK_UserTypes_Events]

-- Drop constraints from [dbo].[Users]
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_Users]

-- Drop constraints from [dbo].[Tags]
ALTER TABLE [dbo].[Tags] DROP CONSTRAINT [FK_Tags_Users]

-- Drop constraints from [dbo].[Redemptions]
ALTER TABLE [dbo].[Redemptions] DROP CONSTRAINT [FK_Redemptions_Tokens]
ALTER TABLE [dbo].[Redemptions] DROP CONSTRAINT [FK_Redemptions_Users]

-- Drop constraints from [dbo].[Messages]
ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [FK_Messages_Users]

-- Drop constraints from [dbo].[Friendships]
ALTER TABLE [dbo].[Friendships] DROP CONSTRAINT [FK_Friendships_Users_Befriendee]
ALTER TABLE [dbo].[Friendships] DROP CONSTRAINT [FK_Friendships_Users_Befriender]

-- Drop constraints from [dbo].[Tokens]
ALTER TABLE [dbo].[Tokens] DROP CONSTRAINT [FK_Tokens_Campaigns]

-- Drop constraints from [dbo].[Sessions]
ALTER TABLE [dbo].[Sessions] DROP CONSTRAINT [FK_Sessions_Events]

-- Drop constraint FK_Sats_Sessions from [dbo].[Sats]
ALTER TABLE [dbo].[Sats] DROP CONSTRAINT [FK_Sats_Sessions]

-- Drop constraints from [dbo].[Seats]
ALTER TABLE [dbo].[Seats] DROP CONSTRAINT [FK_Seats_Events]

-- Drop constraint FK_Sats_Seats from [dbo].[Sats]
ALTER TABLE [dbo].[Sats] DROP CONSTRAINT [FK_Sats_Seats]

-- Add row to [dbo].[Campaigns]
SET IDENTITY_INSERT [dbo].[Campaigns] ON
INSERT INTO [dbo].[Campaigns] ([CampaignID], [Name], [Value]) VALUES (1, 'Are you a winner?', 10)
SET IDENTITY_INSERT [dbo].[Campaigns] OFF

-- Add row to [dbo].[Events]
SET IDENTITY_INSERT [dbo].[Events] ON
INSERT INTO [dbo].[Events] ([EventID], [Name], [Slug]) VALUES (1, 'Test Event', 'testevent')
SET IDENTITY_INSERT [dbo].[Events] OFF

-- Add row to [dbo].[Seats]
SET IDENTITY_INSERT [dbo].[Seats] ON
INSERT INTO [dbo].[Seats] ([SeatID], [SeatNumber], [Row], [Section], [EventID], [Code]) VALUES (1, '1 ', 'A ', 'Circle', 1, 'abc  ')
SET IDENTITY_INSERT [dbo].[Seats] OFF

-- Add row to [dbo].[Sessions]
SET IDENTITY_INSERT [dbo].[Sessions] ON
INSERT INTO [dbo].[Sessions] ([SessionID], [EventID], [Name], [Start], [Finish]) VALUES (1, 1, 'How to rule the world', '2009-08-12 17:00:00.000', '2009-08-12 18:00:00.000')
SET IDENTITY_INSERT [dbo].[Sessions] OFF

-- Add row to [dbo].[Tokens]
SET IDENTITY_INSERT [dbo].[Tokens] ON
INSERT INTO [dbo].[Tokens] ([TokenID], [CampaignID], [Token]) VALUES (1, 1, 'IWIN')
SET IDENTITY_INSERT [dbo].[Tokens] OFF

-- Add row to [dbo].[Redemptions]
SET IDENTITY_INSERT [dbo].[Redemptions] ON
INSERT INTO [dbo].[Redemptions] ([RedemptionID], [Username], [TokenID], [Created]) VALUES (9, 'helloapptest1', 1, '2009-08-13 15:52:49.483')
SET IDENTITY_INSERT [dbo].[Redemptions] OFF

-- Add rows to [dbo].[UserTypes]
INSERT INTO [dbo].[UserTypes] ([UserTypeID], [EventID], [Name], [Ordering], [DefaultColour], [BurningColour], [HotColour], [WarmColour]) VALUES ('biz', 1, N'Biz', 3, '0000cc', '0000ff', '0000ee', '0000dd')
INSERT INTO [dbo].[UserTypes] ([UserTypeID], [EventID], [Name], [Ordering], [DefaultColour], [BurningColour], [HotColour], [WarmColour]) VALUES ('des', 1, N'Design', 2, '00cc00', '00ff00', '00ee00', '00dd00')
INSERT INTO [dbo].[UserTypes] ([UserTypeID], [EventID], [Name], [Ordering], [DefaultColour], [BurningColour], [HotColour], [WarmColour]) VALUES ('dev', 1, N'Dev', 1, 'cc0000', 'ff0000', 'ee0000', 'dd0000')
-- Operation applied to 3 rows out of 3

-- Add constraints to [dbo].[UserTypes]
ALTER TABLE [dbo].[UserTypes] ADD CONSTRAINT [FK_UserTypes_Events] FOREIGN KEY ([EventID]) REFERENCES [dbo].[Events] ([EventID])

-- Add constraints to [dbo].[Users]
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [FK_Users_Users] FOREIGN KEY ([UserTypeID]) REFERENCES [dbo].[UserTypes] ([UserTypeID])

-- Add constraints to [dbo].[Tags]
ALTER TABLE [dbo].[Tags] ADD CONSTRAINT [FK_Tags_Users] FOREIGN KEY ([Username]) REFERENCES [dbo].[Users] ([Username])

-- Add constraints to [dbo].[Redemptions]
ALTER TABLE [dbo].[Redemptions] ADD CONSTRAINT [FK_Redemptions_Tokens] FOREIGN KEY ([TokenID]) REFERENCES [dbo].[Tokens] ([TokenID])
ALTER TABLE [dbo].[Redemptions] ADD CONSTRAINT [FK_Redemptions_Users] FOREIGN KEY ([Username]) REFERENCES [dbo].[Users] ([Username])

-- Add constraints to [dbo].[Messages]
ALTER TABLE [dbo].[Messages] ADD CONSTRAINT [FK_Messages_Users] FOREIGN KEY ([Username]) REFERENCES [dbo].[Users] ([Username])

-- Add constraints to [dbo].[Friendships]
ALTER TABLE [dbo].[Friendships] ADD CONSTRAINT [FK_Friendships_Users_Befriendee] FOREIGN KEY ([Befriendee]) REFERENCES [dbo].[Users] ([Username])
ALTER TABLE [dbo].[Friendships] ADD CONSTRAINT [FK_Friendships_Users_Befriender] FOREIGN KEY ([Befriender]) REFERENCES [dbo].[Users] ([Username])

-- Add constraints to [dbo].[Tokens]
ALTER TABLE [dbo].[Tokens] ADD CONSTRAINT [FK_Tokens_Campaigns] FOREIGN KEY ([CampaignID]) REFERENCES [dbo].[Campaigns] ([CampaignID])

-- Add constraints to [dbo].[Sessions]
ALTER TABLE [dbo].[Sessions] ADD CONSTRAINT [FK_Sessions_Events] FOREIGN KEY ([EventID]) REFERENCES [dbo].[Events] ([EventID])

-- Add constraint FK_Sats_Sessions to [dbo].[Sats]
ALTER TABLE [dbo].[Sats] WITH NOCHECK ADD CONSTRAINT [FK_Sats_Sessions] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Sessions] ([SessionID])

-- Add constraints to [dbo].[Seats]
ALTER TABLE [dbo].[Seats] ADD CONSTRAINT [FK_Seats_Events] FOREIGN KEY ([EventID]) REFERENCES [dbo].[Events] ([EventID])

-- Add constraint FK_Sats_Seats to [dbo].[Sats]
ALTER TABLE [dbo].[Sats] WITH NOCHECK ADD CONSTRAINT [FK_Sats_Seats] FOREIGN KEY ([SeatID]) REFERENCES [dbo].[Seats] ([SeatID])
COMMIT TRANSACTION
GO

-- Reseed identity on [dbo].[Redemptions]
DBCC CHECKIDENT('[dbo].[Redemptions]', RESEED, 9)
GO

-- Reseed identity on [dbo].[QueuedTweets]
DBCC CHECKIDENT('[dbo].[QueuedTweets]', RESEED, 572)
GO

-- Reseed identity on [dbo].[Tokens]
DBCC CHECKIDENT('[dbo].[Tokens]', RESEED, 1)
GO

-- Reseed identity on [dbo].[Sessions]
DBCC CHECKIDENT('[dbo].[Sessions]', RESEED, 1)
GO

-- Reseed identity on [dbo].[Seats]
DBCC CHECKIDENT('[dbo].[Seats]', RESEED, 1)
GO

-- Reseed identity on [dbo].[TideMarks]
DBCC CHECKIDENT('[dbo].[TideMarks]', RESEED, 75)
GO

-- Reseed identity on [dbo].[Events]
DBCC CHECKIDENT('[dbo].[Events]', RESEED, 1)
GO

-- Reseed identity on [dbo].[Campaigns]
DBCC CHECKIDENT('[dbo].[Campaigns]', RESEED, 1)
GO
