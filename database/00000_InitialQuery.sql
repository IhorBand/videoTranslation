DECLARE @SQLString NVARCHAR(500); 

SELECT 
    @SQLString = 'ALTER TABLE [' +  OBJECT_SCHEMA_NAME(parent_object_id) +
    '].[' + OBJECT_NAME(parent_object_id) + 
    '] DROP CONSTRAINT [' + name + ']'
FROM sys.foreign_keys
WHERE referenced_object_id = object_id('dbo.L_FileType')

EXECUTE sp_executesql @SQLString
GO
DROP TABLE IF EXISTS dbo.[L_FileType]
GO

DECLARE @SQLString NVARCHAR(500); 

SELECT 
    @SQLString = 'ALTER TABLE [' +  OBJECT_SCHEMA_NAME(parent_object_id) +
    '].[' + OBJECT_NAME(parent_object_id) + 
    '] DROP CONSTRAINT [' + name + ']'
FROM sys.foreign_keys
WHERE referenced_object_id = object_id('dbo.L_VideoType')

EXECUTE sp_executesql @SQLString
GO
DROP TABLE IF EXISTS dbo.[L_VideoType]
GO

DECLARE @SQLString NVARCHAR(500); 

SELECT 
    @SQLString = 'ALTER TABLE [' +  OBJECT_SCHEMA_NAME(parent_object_id) +
    '].[' + OBJECT_NAME(parent_object_id) + 
    '] DROP CONSTRAINT [' + name + ']'
FROM sys.foreign_keys
WHERE referenced_object_id = object_id('dbo.FileServer')

EXECUTE sp_executesql @SQLString
GO
DROP TABLE IF EXISTS dbo.[FileServer]
GO

DECLARE @SQLString NVARCHAR(500); 

SELECT 
    @SQLString = 'ALTER TABLE [' +  OBJECT_SCHEMA_NAME(parent_object_id) +
    '].[' + OBJECT_NAME(parent_object_id) + 
    '] DROP CONSTRAINT [' + name + ']'
FROM sys.foreign_keys
WHERE referenced_object_id = object_id('dbo.File')

EXECUTE sp_executesql @SQLString
GO
DROP TABLE IF EXISTS dbo.[File]
GO

DECLARE @SQLString NVARCHAR(500); 

SELECT 
    @SQLString = 'ALTER TABLE [' +  OBJECT_SCHEMA_NAME(parent_object_id) +
    '].[' + OBJECT_NAME(parent_object_id) + 
    '] DROP CONSTRAINT [' + name + ']'
FROM sys.foreign_keys
WHERE referenced_object_id = object_id('dbo.VideoInfo')

EXECUTE sp_executesql @SQLString
GO
DROP TABLE IF EXISTS dbo.[VideoInfo]
GO

DECLARE @SQLString NVARCHAR(500); 

SELECT 
    @SQLString = 'ALTER TABLE [' +  OBJECT_SCHEMA_NAME(parent_object_id) +
    '].[' + OBJECT_NAME(parent_object_id) + 
    '] DROP CONSTRAINT [' + name + ']'
FROM sys.foreign_keys
WHERE referenced_object_id = object_id('dbo.VideoFile')

EXECUTE sp_executesql @SQLString
GO
DROP TABLE IF EXISTS dbo.[VideoFile]
GO

CREATE TABLE [dbo].[L_FileType](
	[Id] TINYINT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[Value] NVARCHAR(200) NOT NULL
)
GO

CREATE TABLE [dbo].[L_VideoType](
	[Id] TINYINT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	[Value] NVARCHAR(200) NOT NULL
)
GO

CREATE TABLE [dbo].[FileServer](
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_FileServer_Id DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
	[Name] NVARCHAR(100) NOT NULL,
	[Path] VARCHAR(200) NOT NULL,
	[Url] VARCHAR(200) NOT NULL
)
GO

CREATE TABLE [dbo].[File](
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_File_Id DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
	[FileName] NVARCHAR(500) NOT NULL,
	[FileServerId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_FileServer_File_FileServerId FOREIGN KEY REFERENCES [dbo].[FileServer](Id),
	[FileTypeId] TINYINT NOT NULL CONSTRAINT FK_FileType_File_FileTypeId FOREIGN KEY REFERENCES [dbo].[L_FileType](Id),
	[Size] BIGINT NULL,
	[Extension] VARCHAR(25) NULL
)
GO

CREATE TABLE [dbo].[VideoInfo](
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_VideoInfo_Id DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
	[Name] NVARCHAR(100) NOT NULL,
	[Description] NVARCHAR(250) NULL
)
GO

CREATE TABLE [dbo].[VideoFile](
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_VideoFile_Id DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
	[VideoInfoId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_VideoInfo_VideoFile_VideoInfoId FOREIGN KEY REFERENCES [dbo].[VideoInfo](Id),
	[FileId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_File_VideoFile_FileId FOREIGN KEY REFERENCES [dbo].[File](Id),
	[VideoTypeId] TINYINT NOT NULL CONSTRAINT FK_VideoType_VideoFile_VideoTypeId FOREIGN KEY REFERENCES [dbo].[L_VideoType](Id),
	[ResolutionWidth] INT NOT NULL,
	[ResolutionHeight] INT NOT NULL
)
GO


INSERT INTO [dbo].[L_FileType]([Value])
VALUES('Image')
INSERT INTO [dbo].[L_FileType]([Value])
VALUES('Video')
INSERT INTO [dbo].[L_FileType]([Value])
VALUES('Text')
GO

INSERT INTO [dbo].[L_VideoType]([Value])
VALUES('Original')
INSERT INTO [dbo].[L_VideoType]([Value])
VALUES('Streamed Converted')
INSERT INTO [dbo].[L_VideoType]([Value])
VALUES('Converted')
GO
