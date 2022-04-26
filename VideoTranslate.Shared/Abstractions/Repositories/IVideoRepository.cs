using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Shared.Abstractions.Repositories
{
    public interface IVideoRepository
    {
        List<Video> GetAllVideos();
    }
}
