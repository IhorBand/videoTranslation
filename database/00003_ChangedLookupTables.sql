INSERT INTO [dbo].[L_FileType]([Value])
VALUES('Other')
GO

INSERT INTO [dbo].[FileServer]([Name], [Path], [Url], [IsActive])
VALUES('Primary Server', '/media/uploads/', 'http://139.162.251.5:5003/', 1)
GO
