CREATE TABLE [dbo].[UserTypes]
(
[UserTypeID] [char] (3) COLLATE Latin1_General_CI_AS NOT NULL,
[EventID] [int] NOT NULL,
[Name] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
[DefaultColour] [char] (6) COLLATE Latin1_General_CI_AS NOT NULL,
[BurningColour] [char] (6) COLLATE Latin1_General_CI_AS NOT NULL,
[HotColour] [char] (6) COLLATE Latin1_General_CI_AS NOT NULL,
[WarmColour] [char] (6) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserTypes] ADD CONSTRAINT [PK_UserTypes] PRIMARY KEY CLUSTERED  ([UserTypeID]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserTypes] ADD CONSTRAINT [FK_UserTypes_Events] FOREIGN KEY ([EventID]) REFERENCES [dbo].[Events] ([EventID])
GO
