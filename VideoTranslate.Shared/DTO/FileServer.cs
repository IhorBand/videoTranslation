using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTranslate.Shared.DTO
{
    public class FileServer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}
