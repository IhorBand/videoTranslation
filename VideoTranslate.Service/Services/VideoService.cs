using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoTranslate.Shared.Abstractions.Repositories;
using VideoTranslate.Shared.Abstractions.Services;
using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Service.Services
{
    public class VideoService : BaseService, IVideoService
    {
        public const string ActivitySourceName = nameof(VideoService);

        private static readonly Version Version = typeof(VideoService).Assembly.GetName().Version;
        private static readonly ActivitySource ActivitySource = new ActivitySource(ActivitySourceName, Version?.ToString());

        private readonly IVideoRepository videoRepository;
        private readonly ILogger<VideoService> logger;

        public VideoService(
            IVideoRepository videoRepository,
            ILogger<VideoService> logger)
        {
            this.videoRepository = videoRepository ?? throw new ArgumentNullException(nameof(videoRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public VideoInfo GetVideoById(Guid videoInfoId)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoService),
                () =>
                {
                    return this.videoRepository.GetVideoById(videoInfoId);
                },
                nameof(this.GetVideoById));
        }

        public List<VideoInfo> GetAllVideos()
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoService),
                () =>
                {
                    return this.videoRepository.GetAllVideos();
                },
                nameof(this.GetAllVideos));
        }

        public VideoInfo InsertVideo(VideoInfo videoInfo)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoService),
                () =>
                {
                    var videoInfoId = this.videoRepository.InsertVideo(videoInfo);
                    var videoInfoInserted = this.videoRepository.GetVideoById(videoInfoId);
                    return videoInfoInserted;
                },
                nameof(this.InsertVideo));
        }
    }
}
