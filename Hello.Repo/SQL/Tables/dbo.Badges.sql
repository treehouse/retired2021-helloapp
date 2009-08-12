CREATE TABLE [dbo].[Badges]
(
[BadgeID] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[ImageUrl] [varchar] (100) COLLATE Latin1_General_CI_AS NOT NULL
)

GO
ALTER TABLE [dbo].[Badges] ADD CONSTRAINT [PK_Badges] PRIMARY KEY CLUSTERED  ([BadgeID]) ON [PRIMARY]
GO
