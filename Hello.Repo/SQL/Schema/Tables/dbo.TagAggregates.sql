CREATE TABLE [dbo].[TagAggregates]
(
[TagAggregateID] [int] NOT NULL IDENTITY(1, 1),
[Tag] [varchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
[UserTypeID] [char] (3) COLLATE Latin1_General_CI_AS NULL,
[Count] [int] NOT NULL
)

GO
