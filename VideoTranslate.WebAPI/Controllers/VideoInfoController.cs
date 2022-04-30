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

        public VideoInfoController(
            ILogger<VideoInfoController> logger,
            IMapper mapper,
            IVideoInfoService videoService,
            IFileService fileService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.videoService = videoService;
            this.fileService = fileService;
        }

        [HttpGet("All")]
        public IEnumerable<VideoInfo> Get()
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
            var file = this.fileService.UploadVideoFile(this.Request.Form.Files[0]);
            var fileModel = this.mapper.Map<VideoInfo>(file);
            return fileModel;
        }

        [HttpPost("{videoInfoId}/UploadThumbnail")]
        public VideoInfo UploadThumbnail([FromRoute(Name = "videoInfoId")] Guid videoInfoId)
        {
            var file = this.fileService.UploadThumbnail(this.Request.Form.Files[0], videoInfoId);
            var fileModel = this.mapper.Map<VideoInfo>(file);
            return fileModel;
        }
    }
}
