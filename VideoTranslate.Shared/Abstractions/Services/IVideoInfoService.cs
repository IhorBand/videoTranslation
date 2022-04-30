using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Shared.Abstractions.Services
{
    public interface IVideoInfoService
    {
        VideoInfo GetVideoInfoById(Guid videoInfoId);
        List<VideoInfo> GetAllVideoInfos();
        void UpdateVideoInfo(VideoInfo videoInfo);
        VideoInfo InsertVideoInfo(VideoInfo videoInfo);
    }
}
