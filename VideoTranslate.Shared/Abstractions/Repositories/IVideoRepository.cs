using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Shared.Abstractions.Repositories
{
    public interface IVideoRepository
    {
        VideoInfo GetVideoById(Guid videoInfoId);
        List<VideoInfo> GetAllVideos();
        Guid InsertVideo(VideoInfo videoInfo);
    }
}
