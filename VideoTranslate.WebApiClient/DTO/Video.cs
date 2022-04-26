using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTranslate.WebApiClient.DTO
{
    public record Video
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Video()
        {
            this.Name = string.Empty;
        }
    }
}
