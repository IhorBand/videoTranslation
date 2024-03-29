﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoTranslate.Shared.Abstractions.Repositories;
using VideoTranslate.Shared.Abstractions.Services;
using VideoTranslate.Shared.Abstractions.Services.MQServices;
using VideoTranslate.Shared.DTO;
using VideoTranslate.Shared.DTO.MQModels;

namespace VideoTranslate.Service.Services
{
    public class VideoInfoService : BaseService, IVideoInfoService
    {
        public const string ActivitySourceName = nameof(VideoInfoService);

        private static readonly Version Version = typeof(VideoInfoService).Assembly.GetName().Version;
        private static readonly ActivitySource ActivitySource = new ActivitySource(ActivitySourceName, Version?.ToString());

        private readonly IFFmpegQueueService ffmpegQueueService;

        private readonly IVideoInfoRepository videoRepository;
        private readonly ILogger<VideoInfoService> logger;

        public VideoInfoService(
            IFFmpegQueueService ffmpegQueueService,
            IVideoInfoRepository videoRepository,
            ILogger<VideoInfoService> logger)
        {
            this.ffmpegQueueService = ffmpegQueueService ?? throw new ArgumentNullException(nameof(ffmpegQueueService));
            this.videoRepository = videoRepository ?? throw new ArgumentNullException(nameof(videoRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public VideoInfo GetVideoInfoById(Guid videoInfoId)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoInfoService),
                () =>
                {
                    return this.videoRepository.GetVideoInfoById(videoInfoId);
                },
                nameof(this.GetVideoInfoById));
        }

        public List<VideoInfo> GetAllVideoInfos()
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoInfoService),
                () =>
                {
                    return this.videoRepository.GetAllVideoInfos();
                },
                nameof(this.GetAllVideoInfos));
        }

        public void UpdateVideoInfo(VideoInfo videoInfo)
        {
            this.TraceAction(
                ActivitySource,
                nameof(VideoInfoService),
                () =>
                {
                    this.videoRepository.UpdateVideoInfo(videoInfo);
                },
                nameof(this.InsertVideoInfo));
        }

        public void SendVideoConvertRecognizeCommand(ConvertVideoRecognizeCommand convertVideoRecognizeCommand)
        {
            this.TraceAction(
                ActivitySource,
                nameof(VideoInfoService),
                () =>
                {
                    this.ffmpegQueueService.SendConvertVideoRecognizeCommand(convertVideoRecognizeCommand);
                },
                nameof(this.SendVideoConvertRecognizeCommand));
        }

        public VideoInfo InsertVideoInfo(VideoInfo videoInfo)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(VideoInfoService),
                () =>
                {
                    var videoInfoId = this.videoRepository.InsertVideoInfo(videoInfo);
                    var videoInfoInserted = this.videoRepository.GetVideoInfoById(videoInfoId);
                    return videoInfoInserted;
                },
                nameof(this.InsertVideoInfo));
        }
    }
}
