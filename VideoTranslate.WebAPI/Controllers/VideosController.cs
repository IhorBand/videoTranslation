using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideoTranslate.Shared.Abstractions.Services;
using VideoTranslate.WebApiClient.DTO;

namespace VideoTranslate.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideosController : ControllerBase
    {
        private readonly ILogger<VideosController> logger;
        private readonly IVideoService videoService;
        private readonly IMapper mapper;

        public VideosController(
            ILogger<VideosController> logger,
            IVideoService videoService,
            IMapper mapper)
        {
            this.logger = logger;
            this.videoService = videoService;
            this.mapper = mapper;
        }

        [HttpGet("All")]
        public IEnumerable<Video> Get()
        {
            this.logger.LogInformation("VideoController Started");
            var videos = this.videoService.GetAllVideos();
            this.logger.LogError("Wow, we are trying to map properties.");
            var videoModels = this.mapper.Map<Video[]>(videos);
            this.logger.LogInformation("quiting VideoController");
            return videoModels;
        }
    }
}
