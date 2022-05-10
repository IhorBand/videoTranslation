using VideoTranslate.Shared.DTO;
using VideoTranslate.Shared.DTO.MQModels;

namespace VideoTranslate.Shared.Abstractions.Services
{
    public interface IVideoInfoService
    {
        VideoInfo GetVideoInfoById(Guid videoInfoId);
        List<VideoInfo> GetAllVideoInfos();
        void UpdateVideoInfo(VideoInfo videoInfo);
        void SendVideoConvertRecognizeCommand(ConvertVideoRecognizeCommand convertVideoRecognizeCommand);
        VideoInfo InsertVideoInfo(VideoInfo videoInfo);
    }
}
