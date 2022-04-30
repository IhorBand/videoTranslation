using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Shared.Abstractions.Repositories
{
    public interface IFileServerRepository
    {
        FileServer GetActiveFileServer();
    }
}
