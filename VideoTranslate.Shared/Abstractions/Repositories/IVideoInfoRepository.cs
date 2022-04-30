using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Shared.Abstractions.Repositories
{
    public interface IVideoInfoRepository
    {
        VideoInfo GetVideoInfoById(Guid videoInfoId);
        List<VideoInfo> GetAllVideoInfos();
        void UpdateVideoInfo(VideoInfo videoInfo);
        Guid InsertVideoInfo(VideoInfo videoInfo);
    }
}
