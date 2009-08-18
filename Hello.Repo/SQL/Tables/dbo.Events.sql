CREATE TABLE [dbo].[Events]
(
[EventID] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (100) COLLATE Latin1_General_CI_AS NOT NULL,
[Slug] [varchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
[Start] [datetime] NOT NULL CONSTRAINT [DF_Events_Start] DEFAULT (((2001)-(1))-(1)),
[End] [datetime] NOT NULL CONSTRAINT [DF_Events_End] DEFAULT (((2001)-(1))-(1)),
[HiFiveLimit] [int] NOT NULL CONSTRAINT [DF_Events_HiFiveLimit] DEFAULT ((5))
)


GO
ALTER TABLE [dbo].[Events] ADD CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED  ([EventID]) ON [PRIMARY]
GO
