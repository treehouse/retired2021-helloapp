CREATE TABLE [dbo].[Tags]
(
[Name] [varchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
[Username] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[Created] [datetime] NOT NULL
)

ALTER TABLE [dbo].[Tags] ADD 
CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED  ([Name], [Username])
GO

ALTER TABLE [dbo].[Tags] ADD CONSTRAINT [FK_Tags_Users] FOREIGN KEY ([Username]) REFERENCES [dbo].[Users] ([Username])
GO
