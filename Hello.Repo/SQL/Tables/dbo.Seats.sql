CREATE TABLE [dbo].[Seats]
(
[SeatID] [int] NOT NULL IDENTITY(1, 1),
[Row] [int] NOT NULL,
[Column] [int] NOT NULL,
[EventID] [int] NOT NULL,
[Code] [char] (5) COLLATE Latin1_General_CI_AS NOT NULL
)


ALTER TABLE [dbo].[Seats] ADD 
CONSTRAINT [PK_Seats] PRIMARY KEY CLUSTERED  ([SeatID])

GO

ALTER TABLE [dbo].[Seats] ADD CONSTRAINT [FK_Seats_Events] FOREIGN KEY ([EventID]) REFERENCES [dbo].[Events] ([EventID])
GO
