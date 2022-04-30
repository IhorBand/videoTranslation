using Microsoft.AspNetCore.Http;
using VideoTranslate.Shared.DTO;

namespace VideoTranslate.Shared.Abstractions.Services
{
    public interface IFileService
    {
        VideoInfo UploadVideoFile(IFormFile file);
        VideoInfo UploadThumbnail(IFormFile file, Guid videoInfoId);
    }
}
