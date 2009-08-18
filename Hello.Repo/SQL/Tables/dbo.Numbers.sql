CREATE TABLE [dbo].[Numbers]
(
[Number] [int] NOT NULL
)
GO
ALTER TABLE [dbo].[Numbers] ADD CONSTRAINT [PK_Numbers] PRIMARY KEY CLUSTERED  ([Number])
GO
