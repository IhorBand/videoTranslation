using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTranslate.WebApiClient.DTO
{
    public class File
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public Guid FileServerId { get; set; }
        public byte FileTypeId { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
        public string FullPath { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOnUtc { get; set; }

        File()
        {
            this.FileName = string.Empty;
            this.Extension = string.Empty;
            this.FullPath = string.Empty;
            this.Url = string.Empty;
        }
    }
}
