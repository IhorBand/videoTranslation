using System.Diagnostics;
using Microsoft.Extensions.Logging;
using VideoTranslate.Shared.Abstractions.Repositories;
using VideoTranslate.Shared.Abstractions.Services;
using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Service.Services
{
    public class VideoFileService : BaseService, IVideoFileService
    {
        public const string ActivitySourceName = nameof(VideoFileService);

        private static readonly Version Version = typeof(VideoFileService).Assembly.GetName().Version;
        private static readonly ActivitySource ActivitySource = new ActivitySource(ActivitySourceName, Version?.ToString());

        private readonly ILogger<VideoFileService> logger;
        private readonly IFileRepository fileRepository;
        private readonly IVideoFileRepository videoFileRepository;

        public VideoFileService(
            ILogger<VideoFileService> logger,
            IFileServerRepository fileServerRepository,
            IFileRepository fileRepository,
            IVideoInfoRepository videoInfoRepository,
            IVideoFileRepository videoFileRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.fileRepository = fileRepository;
            this.videoFileRepository = videoFileRepository;
        }

        public IEnumerable<VideoFile> GetVideoFilesByVideoInfo(Guid videoInfoId)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoFileService),
                () =>
                {
                    var videoFiles = this.videoFileRepository.GetVideoFilesByVideoInfo(videoInfoId);
                    return videoFiles;
                },
                nameof(this.GetVideoFilesByVideoInfo));
        }
    }
}
