CREATE TABLE [dbo].[UserBadges]
(
[BadgeID] [int] NOT NULL,
[Username] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[Awarded] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserBadges] ADD CONSTRAINT [PK_UserBadges] PRIMARY KEY CLUSTERED  ([BadgeID], [Username]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserBadges] ADD CONSTRAINT [FK_UserBadges_Badges] FOREIGN KEY ([BadgeID]) REFERENCES [dbo].[Badges] ([BadgeID])
GO
ALTER TABLE [dbo].[UserBadges] ADD CONSTRAINT [FK_UserBadges_Users] FOREIGN KEY ([Username]) REFERENCES [dbo].[Users] ([Username])
GO
