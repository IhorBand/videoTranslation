using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Shared.Abstractions.Services
{
    public interface IVideoService
    {
        VideoInfo GetVideoById(Guid videoInfoId);
        List<VideoInfo> GetAllVideos();
        VideoInfo InsertVideo(VideoInfo videoInfo);
    }
}
