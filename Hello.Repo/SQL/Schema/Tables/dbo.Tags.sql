CREATE TABLE [dbo].[Tags]
(
[Tag] [varchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
[Username] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[Created] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Tags] ADD CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED  ([Tag], [Username]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Tags] ADD CONSTRAINT [FK_Tags_Users] FOREIGN KEY ([Username]) REFERENCES [dbo].[Users] ([Username])
GO
