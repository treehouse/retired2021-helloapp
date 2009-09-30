IF NOT EXISTS (SELECT * FROM master.dbo.syslogins WHERE loginname = N'hello_web')
CREATE LOGIN [hello_web] WITH PASSWORD = 'p@ssw0rd'
GO
CREATE USER [hello_web] FOR LOGIN [hello_web] WITH DEFAULT_SCHEMA=[dbo]
GO
GRANT CONNECT TO [hello_web]
