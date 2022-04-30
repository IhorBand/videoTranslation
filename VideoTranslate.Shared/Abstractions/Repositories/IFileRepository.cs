using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Shared.Abstractions.Repositories
{
    public interface IFileRepository
    {
        DTO.File GetFile(Guid id);
        Guid InsertFile(DTO.File file);
    }
}
