CREATE TABLE [dbo].[Messages]
(
[Username] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[Message] [nvarchar] (140) COLLATE Latin1_General_CI_AS NOT NULL,
[Offensive] [bit] NOT NULL CONSTRAINT [DF_Messages_Offensive] DEFAULT ((0))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Messages] ADD CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED  ([Username]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Messages] ADD CONSTRAINT [FK_Messages_Users] FOREIGN KEY ([Username]) REFERENCES [dbo].[Users] ([Username])
GO
