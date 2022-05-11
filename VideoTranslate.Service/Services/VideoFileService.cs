using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using VideoTranslate.Shared.Abstractions.Repositories;
using VideoTranslate.Shared.Abstractions.Services;
using VideoTranslate.Shared.DTO;
using VideoTranslate.Shared.DTO.Configuration;

namespace VideoTranslate.Service.Services
{
    public class VideoFileService : BaseService, IVideoFileService
    {
        public const string ActivitySourceName = nameof(VideoFileService);

        private static readonly Version Version = typeof(VideoFileService).Assembly.GetName().Version;
        private static readonly ActivitySource ActivitySource = new ActivitySource(ActivitySourceName, Version?.ToString());

        private readonly ILogger<VideoFileService> logger;
        private readonly RabbitMQConfiguration rabbitMQConfiguration;
        private readonly IFileRepository fileRepository;
        private readonly IVideoFileRepository videoFileRepository;

        public VideoFileService(
            ILogger<VideoFileService> logger,
            RabbitMQConfiguration rabbitMQConfiguration,
            IFileRepository fileRepository,
            IVideoFileRepository videoFileRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.rabbitMQConfiguration = rabbitMQConfiguration ?? throw new ArgumentNullException(nameof(rabbitMQConfiguration));
            this.fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
            this.videoFileRepository = videoFileRepository ?? throw new ArgumentNullException(nameof(videoFileRepository));
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

        public VideoFile GetOriginalVideoByVideoInfoId(Guid videoInfoId)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoFileService),
                () =>
                {
                    var videoFile = this.videoFileRepository.GetOriginalVideoByVideoInfoId(videoInfoId);
                    return videoFile;
                },
                nameof(this.GetOriginalVideoByVideoInfoId));
        }
    }
}
