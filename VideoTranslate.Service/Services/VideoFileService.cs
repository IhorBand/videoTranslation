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

        public bool SendMessageToRabbitMQ(string message)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoFileService),
                () =>
                {
                    var factory = new ConnectionFactory()
                    {
                        HostName = this.rabbitMQConfiguration.HostName,
                        UserName = this.rabbitMQConfiguration.User,
                        Password = this.rabbitMQConfiguration.Password
                    };

                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(
                            queue: "hello",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(
                            exchange: string.Empty,
                            routingKey: "hello",
                            basicProperties: null,
                            body: body);
                    }

                    return true;
                },
                nameof(this.SendMessageToRabbitMQ));
        }
    }
}
