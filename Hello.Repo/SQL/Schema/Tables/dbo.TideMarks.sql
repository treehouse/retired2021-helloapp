CREATE TABLE [dbo].[TideMarks]
(
[Name] [varchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
[LastId] [int] NOT NULL
)

ALTER TABLE [dbo].[TideMarks] ADD 
CONSTRAINT [PK_TideMarks] PRIMARY KEY CLUSTERED  ([Name])
GO
