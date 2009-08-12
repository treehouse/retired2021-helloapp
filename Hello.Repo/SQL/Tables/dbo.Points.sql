CREATE TABLE [dbo].[Points]
(
[PointID] [int] NOT NULL,
[Username] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[Issued] [datetime] NOT NULL,
[Amount] [int] NOT NULL,
[Details] [nchar] (20) COLLATE Latin1_General_CI_AS NOT NULL
)

ALTER TABLE [dbo].[Points] ADD 
CONSTRAINT [PK_Points] PRIMARY KEY CLUSTERED  ([PointID])
GO
ALTER TABLE [dbo].[Points] ADD CONSTRAINT [FK_Points_Users] FOREIGN KEY ([Username]) REFERENCES [dbo].[Users] ([Username])
GO
