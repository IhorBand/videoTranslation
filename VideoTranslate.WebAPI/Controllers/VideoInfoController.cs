using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideoTranslate.Shared.Abstractions.Services;
using VideoTranslate.WebApiClient.DTO;

namespace VideoTranslate.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoInfoController : ControllerBase
    {
        private readonly ILogger<VideoInfoController> logger;
        private readonly IMapper mapper;
        private readonly IVideoInfoService videoService;
        private readonly IFileService fileService;
        private readonly IVideoFileService videoFileService;

        public VideoInfoController(
            ILogger<VideoInfoController> logger,
            IMapper mapper,
            IVideoInfoService videoService,
            IFileService fileService,
            IVideoFileService videoFileService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.videoService = videoService;
            this.fileService = fileService;
            this.videoFileService = videoFileService;
        }

        [HttpGet("All")]
        public IEnumerable<VideoInfo> GetAll()
        {
            var videoInfos = this.videoService.GetAllVideoInfos();
            var videoInfoModels = this.mapper.Map<VideoInfo[]>(videoInfos);
            return videoInfoModels;
        }

        [HttpGet("{videoInfoId}")]
        public VideoInfo GetById([FromRoute(Name = "videoInfoId")] Guid videoInfoId)
        {
            var videoInfo = this.videoService.GetVideoInfoById(videoInfoId);
            var videoInfoModel = this.mapper.Map<VideoInfo>(videoInfo);
            return videoInfoModel;
        }

        [HttpGet("{videoInfoId}/VideoFiles")]
        public IEnumerable<VideoFile> GetVideoFilesByVideoInfo([FromRoute(Name = "videoInfoId")] Guid videoInfoId)
        {
            var videoFiles = this.videoFileService.GetVideoFilesByVideoInfo(videoInfoId);
            var videoFileModels = this.mapper.Map<VideoFile[]>(videoFiles);
            return videoFileModels;
        }

        [HttpPost("")]
        public Guid UpdateVideoInfo([FromBody] VideoInfo videoInfo)
        {
            var videoInfoInsertModel = this.mapper.Map<Shared.DTO.VideoInfo>(videoInfo);
            this.videoService.UpdateVideoInfo(videoInfoInsertModel);
            return videoInfoInsertModel.Id;
        }

        [HttpPost("UploadVideo")]
        [RequestSizeLimit(1073741824)]
        [RequestFormLimits(MultipartBodyLengthLimit = 1073741824)]
        public VideoInfo UploadVideoFile()
        {
            var videoInfo = this.fileService.UploadVideoFile(this.Request.Form.Files[0]);
            var videoInfoModel = this.mapper.Map<VideoInfo>(videoInfo);
            return videoInfoModel;
        }

        [HttpPost("{videoInfoId}/UploadThumbnail")]
        public VideoInfo UploadThumbnail([FromRoute(Name = "videoInfoId")] Guid videoInfoId)
        {
            var videoInfo = this.fileService.UploadThumbnail(this.Request.Form.Files[0], videoInfoId);
            var videoInfoModel = this.mapper.Map<VideoInfo>(videoInfo);
            return videoInfoModel;
        }
    }
}
