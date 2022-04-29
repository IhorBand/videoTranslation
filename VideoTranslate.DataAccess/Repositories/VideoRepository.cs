using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoTranslate.Shared.Abstractions.Repositories;
using VideoTranslate.Shared.DTO;
using VideoTranslate.Shared.DTO.Configuration;

namespace VideoTranslate.DataAccess.Repositories
{
    public class VideoRepository : BaseRepository, IVideoRepository
    {
        public const string ActivitySourceName = nameof(VideoRepository);

        private static readonly Version Version = typeof(VideoRepository).Assembly.GetName().Version;
        private static readonly ActivitySource ActivitySource = new ActivitySource(ActivitySourceName, Version?.ToString());
        private readonly ILogger<VideoRepository> logger;

        public VideoRepository(
            ConnectionStringConfiguration connectionStringConfiguration,
            ILogger<VideoRepository> logger)
            : base(connectionStringConfiguration.Main)
        {
            this.logger = logger;
        }

        public VideoInfo GetVideoById(Guid videoInfoId)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoRepository),
                () =>
                {
                    var sql = "SELECT * FROM VideoInfo WHERE Id = @Id";
                    var video = this.ExecuteScalar<VideoInfo>(sql, new { Id = videoInfoId });
                    return video;
                },
                nameof(this.GetVideoById));
        }

        public List<VideoInfo> GetAllVideos()
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoRepository),
                () =>
                {
                    var sql = "SELECT * FROM VideoInfo";
                    var videos = this.Query<VideoInfo>(sql);
                    return videos;
                },
                nameof(this.GetAllVideos));
        }

        public Guid InsertVideo(VideoInfo videoInfo)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoRepository),
                () =>
                {
                    var sql = @"INSERT INTO VideoInfo
                        (
                            [Name], 
                            [Description]
                        ) 
                        VALUES
                        (
                            @Name, 
                            @Description
                        )";

                    var videoId = this.QuerySingle<Guid>(
                        sql,
                        new
                        {
                            Name = videoInfo.Name,
                            Description = videoInfo.Description
                        });

                    return videoId;
                },
                nameof(this.InsertVideo));
        }
    }
}