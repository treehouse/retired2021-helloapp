CREATE TABLE [dbo].[Redemptions]
(
[RedemptionID] [int] NOT NULL IDENTITY(1, 1),
[Username] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[TokenID] [int] NOT NULL,
[Created] [datetime] NOT NULL
)

ALTER TABLE [dbo].[Redemptions] ADD 
CONSTRAINT [PK_Redemptions] PRIMARY KEY CLUSTERED  ([RedemptionID])
GO
ALTER TABLE [dbo].[Redemptions] ADD CONSTRAINT [FK_Redemptions_Tokens] FOREIGN KEY ([TokenID]) REFERENCES [dbo].[Tokens] ([TokenID])
GO
ALTER TABLE [dbo].[Redemptions] ADD CONSTRAINT [FK_Redemptions_Users] FOREIGN KEY ([Username]) REFERENCES [dbo].[Users] ([Username])
GO
