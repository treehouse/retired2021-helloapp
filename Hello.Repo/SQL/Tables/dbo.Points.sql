CREATE TABLE [dbo].[Points]
(
[Username] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[Issued] [datetime] NOT NULL,
[Amount] [int] NOT NULL,
[Details] [nchar] (20) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Points] ADD CONSTRAINT [FK_Points_Users] FOREIGN KEY ([Username]) REFERENCES [dbo].[Users] ([Username])
GO
