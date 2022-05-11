ALTER TABLE [dbo].[VideoFile] ADD IsOriginal BIT NOT NULL CONSTRAINT DF_VideoFile_IsOriginal DEFAULT(0)
GO
