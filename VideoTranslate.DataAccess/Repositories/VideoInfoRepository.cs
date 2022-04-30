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
    public class VideoInfoRepository : BaseRepository, IVideoInfoRepository
    {
        public const string ActivitySourceName = nameof(VideoInfoRepository);

        private static readonly Version Version = typeof(VideoInfoRepository).Assembly.GetName().Version;
        private static readonly ActivitySource ActivitySource = new ActivitySource(ActivitySourceName, Version?.ToString());
        private readonly ILogger<VideoInfoRepository> logger;

        public VideoInfoRepository(
            ConnectionStringConfiguration connectionStringConfiguration,
            ILogger<VideoInfoRepository> logger)
            : base(connectionStringConfiguration.Main)
        {
            this.logger = logger;
        }

        public VideoInfo GetVideoInfoById(Guid videoInfoId)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoInfoRepository),
                () =>
                {
                    var sql = "SELECT * FROM [dbo].[VideoInfo] WHERE Id = @Id";
                    var video = this.QuerySingle<VideoInfo>(sql, new { Id = videoInfoId });
                    return video;
                },
                nameof(this.GetVideoInfoById));
        }

        public List<VideoInfo> GetAllVideoInfos()
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoInfoRepository),
                () =>
                {
                    var sql = "SELECT * FROM [dbo].[VideoInfo]";
                    var videos = this.Query<VideoInfo>(sql);
                    return videos;
                },
                nameof(this.GetAllVideoInfos));
        }

        public void UpdateVideoInfo(VideoInfo videoInfo)
        {
            this.TraceAction(
                ActivitySource,
                nameof(VideoInfoRepository),
                () =>
                {
                    var sql = @"UPDATE [dbo].[VideoInfo] 
                        SET
                            Name = @Name,
                            Description = @Description
                        WHERE Id = @Id";

                    var prms = new
                    {
                        Id = videoInfo.Id,
                        Name = videoInfo.Name,
                        Description = videoInfo.Description
                    };

                    this.Execute(sql, prms);
                },
                nameof(this.UpdateVideoInfo));
        }

        public Guid InsertVideoInfo(VideoInfo videoInfo)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoInfoRepository),
                () =>
                {
                    var sql = @"INSERT INTO [dbo].[VideoInfo]
                        (
                            [Name], 
                            [Description]
                        )
                        OUTPUT INSERTED.Id
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
                nameof(this.InsertVideoInfo));
        }
    }
}