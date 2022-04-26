using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Shared.Abstractions.Services
{
    public interface IVideoService
    {
        public List<Video> GetAllVideos();
    }
}
