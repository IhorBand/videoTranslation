using VideoTranslate.Shared.DTO.MQModels;

namespace VideoTranslate.Shared.Abstractions.Services.MQServices
{
    public interface IFFmpegQueueService
    {
        void SendConvertVideoRecognizeCommand(ConvertVideoRecognizeCommand convertVideoRecognizeCommand);
    }
}
