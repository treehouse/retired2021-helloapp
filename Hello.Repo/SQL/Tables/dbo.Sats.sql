CREATE TABLE [dbo].[Sats]
(
[Username] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[SessionID] [int] NOT NULL,
[SeatID] [int] NOT NULL
)

ALTER TABLE [dbo].[Sats] ADD
CONSTRAINT [FK_Sats_Seats] FOREIGN KEY ([SeatID]) REFERENCES [dbo].[Seats] ([SeatID])
ALTER TABLE [dbo].[Sats] ADD
CONSTRAINT [FK_Sats_Sessions] FOREIGN KEY ([SessionID]) REFERENCES [dbo].[Sessions] ([SessionID])
GO
ALTER TABLE [dbo].[Sats] ADD CONSTRAINT [PK_Sits] PRIMARY KEY CLUSTERED  ([Username], [SessionID])
GO
ALTER TABLE [dbo].[Sats] ADD CONSTRAINT [FK_Sits_Users] FOREIGN KEY ([Username]) REFERENCES [dbo].[Users] ([Username])
GO
