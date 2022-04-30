using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Shared.Abstractions.Repositories
{
    public interface IVideoFileRepository
    {
        IEnumerable<VideoFile> GetVideoFilesByVideoInfo(Guid id);
        Guid InsertVideoFile(VideoFile videoFile);
    }
}
