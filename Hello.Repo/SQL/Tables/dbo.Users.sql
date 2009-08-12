CREATE TABLE [dbo].[Users]
(
[Username] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[ImageUrl] [varchar] (100) COLLATE Latin1_General_CI_AS NOT NULL,
[Created] [datetime] NOT NULL,
[Updated] [datetime] NOT NULL,
[UserTypeID] [char] (3) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED  ([Username]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [FK_Users_Users] FOREIGN KEY ([UserTypeID]) REFERENCES [dbo].[UserTypes] ([UserTypeID])
GO
