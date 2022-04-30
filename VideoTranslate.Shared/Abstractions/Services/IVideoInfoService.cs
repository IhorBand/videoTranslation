using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Shared.Abstractions.Services
{
    public interface IVideoInfoService
    {
        VideoInfo GetVideoById(Guid videoInfoId);
        List<VideoInfo> GetAllVideos();
        VideoInfo InsertVideo(VideoInfo videoInfo);
    }
}
