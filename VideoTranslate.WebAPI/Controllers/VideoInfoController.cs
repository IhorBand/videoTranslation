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
        private readonly IVideoInfoService videoService;
        private readonly IMapper mapper;

        public VideoInfoController(
            ILogger<VideoInfoController> logger,
            IVideoInfoService videoService,
            IMapper mapper)
        {
            this.logger = logger;
            this.videoService = videoService;
            this.mapper = mapper;
        }

        [HttpGet("All")]
        public IEnumerable<VideoInfo> Get()
        {
            var videoInfos = this.videoService.GetAllVideos();
            var videoInfoModels = this.mapper.Map<VideoInfo[]>(videoInfos);
            return videoInfoModels;
        }

        [HttpGet("{videoInfoId}")]
        public VideoInfo GetById([FromRoute(Name = "videoInfoId")] Guid videoInfoId)
        {
            var videoInfo = this.videoService.GetVideoById(videoInfoId);
            var videoInfoModel = this.mapper.Map<VideoInfo>(videoInfo);
            return videoInfoModel;
        }

        [HttpPut("")]
        public VideoInfo InsertVideoInfo([FromBody] VideoInfo videoInfo)
        {
            var videoInfoInsertModel = this.mapper.Map<Shared.DTO.VideoInfo>(videoInfo);
            var savedVideoInfo = this.videoService.InsertVideo(videoInfoInsertModel);
            var videoInfoModel = this.mapper.Map<VideoInfo>(savedVideoInfo);
            return videoInfoModel;
        }
    }
}
