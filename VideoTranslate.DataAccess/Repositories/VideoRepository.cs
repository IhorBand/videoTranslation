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

        public List<Video> GetAllVideos()
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoRepository),
                () =>
                {
                    var sql = "SELECT * FROM Videos";

                    var videos = this.Query<Video>(sql);

                    return videos;
                },
                nameof(VideoRepository));
        }
    }
}