using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Shared.Abstractions.Services
{
    public interface IVideoFileService
    {
        IEnumerable<VideoFile> GetVideoFilesByVideoInfo(Guid videoInfoId);
        bool SendMessageToRabbitMQ(string message);
    }
}
