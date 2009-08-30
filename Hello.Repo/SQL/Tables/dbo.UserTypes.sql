CREATE TABLE [dbo].[UserTypes]
(
[UserTypeID] [char] (3) COLLATE Latin1_General_CI_AS NOT NULL,
[Name] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
[Ordering] [int] NOT NULL CONSTRAINT [DF_UserTypes_Ordering] DEFAULT ((0)),
[DefaultColour] [char] (6) COLLATE Latin1_General_CI_AS NOT NULL,
[BurningColour] [char] (6) COLLATE Latin1_General_CI_AS NOT NULL,
[HotColour] [char] (6) COLLATE Latin1_General_CI_AS NOT NULL,
[WarmColour] [char] (6) COLLATE Latin1_General_CI_AS NOT NULL
)


GO
ALTER TABLE [dbo].[UserTypes] ADD CONSTRAINT [PK_UserTypes] PRIMARY KEY CLUSTERED  ([UserTypeID]) ON [PRIMARY]
GO
