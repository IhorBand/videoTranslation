using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VideoTranslate.Shared.Abstractions.Repositories;
using VideoTranslate.Shared.Abstractions.Services;
using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Service.Services
{
    public class FileService : BaseService, IFileService
    {
        public const string ActivitySourceName = nameof(FileService);

        private static readonly Version Version = typeof(FileService).Assembly.GetName().Version;
        private static readonly ActivitySource ActivitySource = new ActivitySource(ActivitySourceName, Version?.ToString());

        private readonly ILogger<FileService> logger;
        private readonly IFileServerRepository fileServerRepository;
        private readonly IFileRepository fileRepository;
        private readonly IVideoInfoRepository videoInfoRepository;
        private readonly IVideoFileRepository videoFileRepository;

        public FileService(
            ILogger<FileService> logger,
            IFileServerRepository fileServerRepository,
            IFileRepository fileRepository,
            IVideoInfoRepository videoInfoRepository,
            IVideoFileRepository videoFileRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.fileServerRepository = fileServerRepository;
            this.fileRepository = fileRepository;
            this.videoInfoRepository = videoInfoRepository;
            this.videoFileRepository = videoFileRepository;
        }

        public VideoInfo UploadVideoFile(IFormFile file)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(FileService),
                () =>
                {
                    var fileServer = this.fileServerRepository.GetActiveFileServer();
                    fileServer.Path += "videos/";
                    fileServer.Url += "videos/";

                    var fileExtension = Path.GetExtension(file.FileName);
                    if (string.IsNullOrEmpty(fileExtension))
                    {
                        fileExtension = ".mp4";
                    }

                    var filename = Guid.NewGuid().ToString() + fileExtension;

                    var filePath = Path.Combine(fileServer.Path, filename);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var fileSaveModel = new Shared.DTO.File()
                    {
                        FileName = filename,
                        Extension = fileExtension,
                        FileServerId = fileServer.Id,
                        FileTypeId = FileType.Video,
                        Size = file.Length,
                        FullPath = filePath,
                        Url = fileServer.Url + filename,
                    };

                    var fileId = this.fileRepository.InsertFile(fileSaveModel);

                    var videoInfoId = this.videoInfoRepository.InsertVideoInfo(new VideoInfo()
                    {
                        Name = string.Empty,
                        Description = string.Empty,
                        ThumbnailFileId = null,
                        ThumbnailUrl = null
                    });

                    var videoFileId = this.videoFileRepository.InsertVideoFile(new VideoFile()
                    {
                        FileId = fileId,
                        VideoInfoId = videoInfoId,
                        VideoTypeId = VideoType.Original,
                        ResolutionHeight = 0,
                        ResolutionWidth = 0
                    });

                    var videoInfo = this.videoInfoRepository.GetVideoInfoById(videoInfoId);

                    return videoInfo;
                },
                nameof(this.UploadVideoFile));
        }

        public VideoInfo UploadThumbnail(IFormFile file, Guid videoInfoId)
        {
            return this.TraceAction(
                ActivitySource,
                nameof(FileService),
                () =>
                {
                    var fileServer = this.fileServerRepository.GetActiveFileServer();
                    fileServer.Path += "images/";
                    fileServer.Url += "images/";

                    var fileExtension = Path.GetExtension(file.FileName);
                    if (string.IsNullOrEmpty(fileExtension))
                    {
                        fileExtension = ".jpg";
                    }

                    var filename = Guid.NewGuid().ToString() + fileExtension;

                    var filePath = Path.Combine(fileServer.Path, filename);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var fileSaveModel = new Shared.DTO.File()
                    {
                        FileName = filename,
                        Extension = fileExtension,
                        FileServerId = fileServer.Id,
                        FileTypeId = FileType.Image,
                        Size = file.Length,
                        FullPath = filePath,
                        Url = fileServer.Url + filename,
                    };

                    var fileId = this.fileRepository.InsertFile(fileSaveModel);

                    var videoInfo = this.videoInfoRepository.GetVideoInfoById(videoInfoId);

                    videoInfo.ThumbnailFileId = fileId;
                    videoInfo.ThumbnailUrl = fileSaveModel.Url;

                    this.videoInfoRepository.UpdateVideoInfo(videoInfo);

                    return videoInfo;
                },
                nameof(this.UploadVideoFile));
        }
    }
}
