using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTranslate.Shared.DTO
{
    public class VideoInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ThumbnailFileId { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}
