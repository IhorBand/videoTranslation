ALTER TABLE [dbo].[FileServer] ADD IsActive BIT NOT NULL CONSTRAINT DF_FileServer_IsActive DEFAULT(0)
GO
