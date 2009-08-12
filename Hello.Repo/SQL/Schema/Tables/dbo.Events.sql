CREATE TABLE [dbo].[Events]
(
[EventID] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (100) COLLATE Latin1_General_CI_AS NOT NULL,
[Slug] [varchar] (20) COLLATE Latin1_General_CI_AS NOT NULL
)

GO
ALTER TABLE [dbo].[Events] ADD CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED  ([EventID]) ON [PRIMARY]
GO
