using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Shared.Abstractions.Repositories
{
    public interface IVideoInfoRepository
    {
        VideoInfo GetVideoById(Guid videoInfoId);
        List<VideoInfo> GetAllVideoInfos();
        Guid InsertVideoInfo(VideoInfo videoInfo);
    }
}
