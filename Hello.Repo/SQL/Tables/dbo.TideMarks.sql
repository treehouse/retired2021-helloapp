CREATE TABLE [dbo].[TideMarks]
(
[TideMarkID] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
[LastID] [bigint] NOT NULL,
[TimeStamp] [datetime] NOT NULL
)

ALTER TABLE [dbo].[TideMarks] ADD 
CONSTRAINT [PK_TideMarks] PRIMARY KEY CLUSTERED  ([TideMarkID])


GO
