CREATE TABLE [dbo].[Users]
(
[Username] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[ImageURL] [varchar] (150) COLLATE Latin1_General_CI_AS NOT NULL,
[Created] [datetime] NOT NULL,
[Updated] [datetime] NOT NULL,
[UserTypeID] [char] (3) COLLATE Latin1_General_CI_AS NULL,
[ShadowAccount] [bit] NOT NULL CONSTRAINT [DF_Users_ShadowAccount] DEFAULT ((0)),
[Followers] [int] NOT NULL CONSTRAINT [DF_Users_Followers] DEFAULT ((0)),
[FullName] [varchar](100) COLLATE Latin1_General_CI_AS NULL
)



GO


ALTER FULLTEXT INDEX ON [dbo].[Users] ENABLE
GO

ALTER TABLE [dbo].[Users] ADD CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED  ([Username]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [FK_Users_Users] FOREIGN KEY ([UserTypeID]) REFERENCES [dbo].[UserTypes] ([UserTypeID])
GO
