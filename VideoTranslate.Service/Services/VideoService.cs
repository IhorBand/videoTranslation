﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public VideoService(
            IVideoRepository videoRepository)
        {
            this.videoRepository = videoRepository ?? throw new ArgumentNullException(nameof(videoRepository));
        }

        public List<Video> GetAllVideos()
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
    }
}