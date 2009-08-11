CREATE TABLE [dbo].[Campaigns]
(
[CampaignID] [int] NOT NULL,
[Name] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[Value] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Campaigns] ADD CONSTRAINT [PK_Campaigns] PRIMARY KEY CLUSTERED  ([CampaignID]) ON [PRIMARY]
GO
