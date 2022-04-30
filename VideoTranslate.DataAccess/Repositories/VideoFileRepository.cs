using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using VideoTranslate.Shared.Abstractions.Repositories;
using VideoTranslate.Shared.DTO;
using VideoTranslate.Shared.DTO.Configuration;

namespace VideoTranslate.DataAccess.Repositories
{
    public class VideoFileRepository : BaseRepository, IVideoFileRepository
    {
        public const string ActivitySourceName = nameof(VideoFileRepository);

        private static readonly Version Version = typeof(VideoFileRepository).Assembly.GetName().Version;
        private static readonly ActivitySource ActivitySource = new ActivitySource(ActivitySourceName, Version?.ToString());
        private readonly ILogger<VideoFileRepository> logger;

        public VideoFileRepository(
            ConnectionStringConfiguration connectionStringConfiguration,
            ILogger<VideoFileRepository> logger)
            : base(connectionStringConfiguration.Main)
        {
            this.logger = logger;
        }

        public IEnumerable<VideoFile> GetVideoFilesByVideoInfo(Guid videoInfoId)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoFileRepository),
                () =>
                {
                    var sql = "SELECT * FROM [dbo].[VideoFile] WHERE VideoInfoId = @VideoInfoId";
                    var videoInfos = this.Query<VideoFile>(sql, new { VideoInfoId = videoInfoId });
                    return videoInfos;
                },
                nameof(this.GetVideoFilesByVideoInfo));
        }

        public Guid InsertVideoFile(VideoFile videoFile)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoFileRepository),
                () =>
                {
                    var sql = @"INSERT INTO [dbo].[VideoFile]
                        (
                            [VideoInfoId], 
                            [FileId],
                            [VideoTypeId],
                            [ResolutionWidth],
                            [ResolutionHeight]
                        )
                        OUTPUT INSERTED.Id
                        VALUES
                        (
                            @VideoInfoId, 
                            @FileId,
                            @VideoTypeId,
                            @ResolutionWidth,
                            @ResolutionHeight
                        )";

                    var videoId = this.QuerySingle<Guid>(
                        sql,
                        new
                        {
                            VideoInfoId = videoFile.VideoInfoId,
                            FileId = videoFile.FileId,
                            VideoTypeId = videoFile.VideoTypeId,
                            ResolutionWidth = videoFile.ResolutionWidth,
                            ResolutionHeight = videoFile.ResolutionHeight,
                        });

                    return videoId;
                },
                nameof(this.InsertVideoFile));
        }
    }
}
