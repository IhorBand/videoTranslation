ALTER TABLE [dbo].[VideoInfo] ADD ThumbnailFileId UNIQUEIDENTIFIER NULL 
GO

ALTER TABLE [VideoInfo]
ADD CONSTRAINT FK_File_VideoInfo_ThumbnailFileId
FOREIGN KEY ([ThumbnailFileId]) REFERENCES [File](Id);
GO


ALTER TABLE [VideoInfo]
ADD ThumbnailUrl VARCHAR(300) NULL
GO
