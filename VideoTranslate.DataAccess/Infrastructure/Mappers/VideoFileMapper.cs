using DapperExtensions.Mapper;
using VideoTranslate.Shared.DTO;

namespace VideoTranslate.DataAccess.Infrastructure.Mappers
{
    public class VideoFileMapper : ClassMapper<VideoFile>
    {
        public VideoFileMapper()
        {
            this.Table("VideoFile");

            /*
            this.Map(videoFile => videoFile.Url).Ignore();
            */

            this.AutoMap();
        }
    }
}
