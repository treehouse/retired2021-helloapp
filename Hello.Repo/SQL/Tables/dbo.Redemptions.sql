CREATE TABLE [dbo].[Redemptions]
(
[Username] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[TokenID] [int] NOT NULL,
[Created] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Redemptions] ADD CONSTRAINT [FK_Redemptions_Tokens] FOREIGN KEY ([TokenID]) REFERENCES [dbo].[Tokens] ([TokenID])
GO
ALTER TABLE [dbo].[Redemptions] ADD CONSTRAINT [FK_Redemptions_Users] FOREIGN KEY ([Username]) REFERENCES [dbo].[Users] ([Username])
GO
