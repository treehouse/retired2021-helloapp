CREATE TABLE [dbo].[Sessions]
(
[SessionID] [int] NOT NULL,
[EventID] [int] NOT NULL,
[Name] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[Start] [datetime] NOT NULL,
[Finish] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Sessions] ADD CONSTRAINT [PK_Sessions] PRIMARY KEY CLUSTERED  ([SessionID]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Sessions] ADD CONSTRAINT [FK_Sessions_Events] FOREIGN KEY ([EventID]) REFERENCES [dbo].[Events] ([EventID])
GO
