IF NOT EXISTS (SELECT * FROM master.dbo.syslogins WHERE loginname = N'hello_bot')
CREATE LOGIN [hello_bot] WITH PASSWORD = 'p@ssw0rd'
GO
CREATE USER [hello_bot] FOR LOGIN [hello_bot] WITH DEFAULT_SCHEMA=[dbo]
GO
GRANT CONNECT TO [hello_bot]
