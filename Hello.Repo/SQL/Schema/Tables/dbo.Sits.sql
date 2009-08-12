CREATE TABLE [dbo].[Sits]
(
[Username] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[SessionID] [int] NOT NULL
)

ALTER TABLE [dbo].[Sits] ADD 
CONSTRAINT [PK_Sits] PRIMARY KEY CLUSTERED  ([Username], [SessionID])
GO
ALTER TABLE [dbo].[Sits] ADD CONSTRAINT [FK_Sits_Users] FOREIGN KEY ([Username]) REFERENCES [dbo].[Users] ([Username])
GO
